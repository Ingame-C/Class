using Class.Manager;
using Class.UI;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Class
{
    public class GameManagerEx : MonoBehaviour
    {

        #region Singleton
        private static GameManagerEx instance;
        public static GameManagerEx Instance { get { return instance; } }


        private void Init()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            SceneManager.sceneLoaded += InitScene;
            SceneManager.sceneUnloaded += ClearScene;
        }

        #endregion


        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            OnStageStartAction -= InitThismanManager;
            OnStageStartAction += InitThismanManager;

            OnStageStartAction -= SetTimer;
            OnStageStartAction += SetTimer;

            OnStageStartAction.Invoke();
        }

        // TODO : 정확한 스테이지 이동이 구현되어야합니다.
        // 해당 함수들은 현재 실패한/클리어한 스테이지 ID를 받고 다음에 이동할 스테이지ID를 구해야합니다.

        private int currentStage = 1;
        public int CurrentStage {  get { return currentStage; } }

        public bool OnStageClear(int clearStageId)
        {
            Managers.Data.SaveClearStage(clearStageId);
            if (SceneManager.GetActiveScene().name != SceneEnums.Game.ToString()) return false;
            if (isLoadingScene) return false;

            MoveStage(Mathf.Clamp(clearStageId + 1, clearStageId, Managers.Resource.GetStageCount()));
            StartCoroutine(LoadSceneAfterClear(SceneEnums.Game));
            return true;
        }

        public bool OnStageFailed(int failedStageId)
        {
            if (SceneManager.GetActiveScene().name != SceneEnums.Game.ToString()) return false;
            if (isLoadingScene) return false;

            MoveStage(Mathf.Clamp(failedStageId - 1, 1, failedStageId));
            StartCoroutine(LoadSceneAfterFail(SceneEnums.Game));
            return true;
        }

        private void MoveStage(int stageId)
        {
            currentStage = stageId;
        }

        public void DirectSceneConversion(SceneEnums sceneEnum)
        {
            SceneManager.LoadScene(Enum.GetName(typeof(SceneEnums), sceneEnum));
        }



        // SerializeField 로 하면 Scene 전환 될 때마다 Missing됩니다.
        // 따라서, Tag 달아서 Find 함수 사용하도록 하겠습니다. 혹시 더 빠른 방안 있으시면 말씀해주세요.

        /** Find in Runtime **/
        [SerializeField] private PlayerController controller;
        [SerializeField] private Door doorToOpen;
        [SerializeField] private Chair startChair;              // 플레이어가 재시작 할때마다 깨어날 의자 필요

        public Chair StartChair { get => startChair; }

        [Header("Game Over")]
        [SerializeField] private ScreenBlocker screenBlocker;
        [SerializeField] private GameObject thismanPrefab;
        [SerializeField] private GameObject thismanManagerPrefab;

        [Header("Timer")]
        [SerializeField] private float maxRemainedTime;
        [SerializeField] private float remainedPlayTime;

        /** Actions **/
        public Action OnStageFailAction { get; set; }
        public Action OnStageClearAction { get; set; }

        public Action OnStageStartAction { get; set; }

        /** State Variables **/
        private bool isLoadingScene = false;

        public PlayerController Controller { get => controller; }

        /** GameObjects **/
        private GameObject thismanManager = null;

        private void InitScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneEnums.Game.ToString()) return;   // 게임씬에만 필요한 Init입니다.
         
            controller = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER).GetComponent<PlayerController>();

            var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
            foreach (GameObject prop in initProps)
            {
                if (prop.GetComponent<Door>() != null) doorToOpen = prop.GetComponent<Door>();
                if (prop.GetComponent<Chair>() != null) startChair = prop.GetComponent<Chair>();
            }
        }

        private void ClearScene(Scene scene)
        {
            if (scene.name != SceneEnums.Game.ToString()) return;
            OnStageClearAction = null;
            OnStageFailAction = null;
        }



        private IEnumerator LoadSceneAfterClear(SceneEnums sceneEnum)
        {
            isLoadingScene = true;
            // TODO : 폭죽 펑!
            OnStageClearAction.Invoke();

            FinThismanManager();
            yield return new WaitForSeconds(1.0f);

            OnStageStartAction.Invoke();
            isLoadingScene = false;
        }

        private IEnumerator LoadSceneAfterFail(SceneEnums sceneEnum)
        {
            isLoadingScene = true;

            // 문열고 기다렸다가 Input Block, Spawn Thisman
            doorToOpen.Interact(controller);
            yield return new WaitForSeconds(0.8f);
 
            SpawnThisman();
            OnStageFailAction.Invoke();
            FinThismanManager();
            yield return new WaitForSeconds(0.8f);

            // 씬 전환
            yield return StartCoroutine(screenBlocker.FadeInCoroutine(1.0f));
            UnityEngine.AsyncOperation async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneEnums), sceneEnum));
            yield return async;
            yield return StartCoroutine(screenBlocker.FadeOutCoroutine(0.5f));

            OnStageStartAction.Invoke();
            isLoadingScene = false;
        }

        private void SpawnThisman()
        {
            GameObject tmpThis = Instantiate(thismanPrefab, doorToOpen.OriginalPosition, Quaternion.identity);
            tmpThis.GetComponent<ThismanController>().SetThismanTarget(controller.transform);

            controller.GetComponent<PlayerController>().thismanState.Thisman = tmpThis.transform;

        }

        private void InitThismanManager()
        {
            thismanManager = Instantiate(thismanManagerPrefab, transform);
            thismanManager.GetComponent<ThismanManager>().Init();
        }


        private void FinThismanManager()
        {
            if (thismanManager != null)
            {
                Destroy(thismanManager); thismanManager = null;
            }
        }



        private bool isTimerSet = false;
        public bool IsTimerSet {  get { return isTimerSet; }  }

        private void SetTimer()
        {
            remainedPlayTime = maxRemainedTime;
            isTimerSet = true;
        }



        // HACK : 테스트 코드입니다.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnStageFailed(currentStage);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                OnStageClear(currentStage);
            }


            if (remainedPlayTime < 0 && isTimerSet)
            {
                isTimerSet = false;
                if(thismanManager != null &&
                        !thismanManager.GetComponent<ThismanManager>().IsComing) OnStageFailed(currentStage);
            }

            remainedPlayTime -= Time.deltaTime;
            

            /** Check clear condition **/
            // HACK : 해당 부분 Func< ... , bool> 사용해서 여러 조건들을 담을 수 있도록 해야합니다.
            // 담는 방식에 대해서는 좀 더 고민해야 할 것 같습니다.
            if (DeskManager.Instance.CheckCleared() && isTimerSet) {
                isTimerSet = false;
                OnStageClear(currentStage);
            }

        }

    }
}