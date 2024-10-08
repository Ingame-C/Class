using Class.StateMachine;
using Class.UI;
using System;
using System.Collections;
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

        // TODO : 정확한 스테이지 이동이 구현되어야합니다.
        // 해당 함수들은 현재 실패한/클리어한 스테이지 ID를 받고 다음에 이동할 스테이지ID를 구해야합니다.

        public bool OnStageClear(int clearStageId)
        {
            if (SceneManager.GetActiveScene().name != SceneEnums.Game.ToString()) return false;
            if (isLoadingScene) return false;

            StartCoroutine(LoadSceneAfterClear(SceneEnums.Game));
            return true;
        }

        public bool OnStageFailed(int faildStageId)
        {
            if (SceneManager.GetActiveScene().name != SceneEnums.Game.ToString()) return false;
            if (isLoadingScene) return false;

            StartCoroutine(LoadSceneAfterFail(SceneEnums.Game));
            return true;
        }

        private void MoveStage(int stageId)
        {

        }

        public void DirectSceneConversion(SceneEnums sceneEnum)
        {
            SceneManager.LoadScene(Enum.GetName(typeof(SceneEnums), sceneEnum));
        }



        // SerializeField 로 하면 Scene 전환 될 때마다 Missing됩니다.
        // 따라서, Tag 달아서 Find 함수 사용하도록 하겠습니다. 혹시 더 빠른 방안 있으시면 말씀해주세요.

        /** Find in Runtime **/
        private PlayerController controller;
        private Door doorToOpen;
        private Chair startChair;              // 플레이어가 재시작 할때마다 깨어날 의자 필요

        public Chair StartChair { get => startChair; }

        [Header("Game Over")]
        [SerializeField] private ScreenBlocker screenBlocker;
        [SerializeField] private GameObject thismanPrefab;


        /** Actions **/
        public Action OnStageFailAction;
        public Action OnStageClearAction;

        /** State Variables **/
        private bool isLoadingScene = false;


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
            OnStageFailAction = null;
            OnStageClearAction = null;
        }



        private IEnumerator LoadSceneAfterClear(SceneEnums sceneEnum)
        {
            isLoadingScene = true;
            OnStageClearAction.Invoke();
            yield return new WaitForSeconds(1.0f);
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
            yield return new WaitForSeconds(0.8f);

            // 씬 전환
            yield return StartCoroutine(screenBlocker.FadeInCoroutine(1.0f));
            AsyncOperation async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneEnums), sceneEnum));
            yield return async;
            yield return StartCoroutine(screenBlocker.FadeOutCoroutine(0.5f));

            isLoadingScene = false;
        }

        public void SpawnThisman()
        {
            GameObject tmpThis = Instantiate(thismanPrefab, doorToOpen.transform.position, Quaternion.identity);
            tmpThis.GetComponent<ThismanController>().SetThismanTarget(controller.transform);

            controller.GetComponent<PlayerController>().thismanState.Thisman = tmpThis.transform;

        }


        // HACK : 테스트 코드입니다.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnStageFailed(1);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                OnStageClear(1);
            }
        }

    }
}