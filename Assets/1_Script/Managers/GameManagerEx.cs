using Class.Manager;
using Class.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Class
{
    public class GameManagerEx : MonoBehaviour
    {
        // 클리어의 조건들을 담을 Func의 List입니다. Init 함수에 초기화 해 두도록 하겠습니다.
        private List<Func<bool>> stageClearConditions;
        private static GameManagerEx instance;
        public static GameManagerEx Instance { get { return instance; } }

        // 싱글톤과 클리어 조건을 제어하는 리전입니다.
        #region Init and Awake
        private void Init()
        {
            #region Singleton
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
            #endregion

            stageClearConditions = new List<Func<bool>>
            {
                () => true,     // 스테이지와 인덱스를 일치시키기 위한 선언입니다.
                () => DeskManager.Instance.CheckCleared(),
                () => LecternManager.Instance.CheckCleared(),
            };

            SceneManager.sceneLoaded += InitScene;
            SceneManager.sceneUnloaded += ClearScene;
            
        }
        
        private void Awake()
        {
            Init();
        }

        #endregion
        
        // 각 스테이지 시작시 작동시켜야 할 함수들을 담습니다.
        private void Start()
        {
            OnStageStartAction -= InitThismanManager;
            OnStageStartAction += InitThismanManager;
            OnStageStartAction -= EffectManager.Instance.ResetEffectLogic;
            OnStageStartAction += EffectManager.Instance.ResetEffectLogic;
            
            // 스테이지 1.
            if (currentStage == 1)
            {
                OnStageStartAction -= DeskManager.Instance.GenerateReflectionOnly;
                OnStageStartAction += DeskManager.Instance.GenerateReflectionOnly;
                OnStageStartAction -= DeskManager.Instance.SetRandomPreset;
                OnStageStartAction += DeskManager.Instance.SetRandomPreset;
            }
            
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
        
        
        // 스테이지에서 게임 오버의 경우.
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
        [SerializeField] private List<Light> directionalLights = new List<Light>();
        [SerializeField] private TVController tvController;
        [SerializeField] private BloodyFloorController floorController;

        public Chair StartChair { get => startChair; }
        public BloodyFloorController FloorController { get => floorController; }
        public List<Light> DirectionalLights { get => directionalLights; }

        [Header("Game Over")]
        [SerializeField] private ScreenBlocker screenBlocker;
        [SerializeField] private GameObject thismanPrefab;
        [SerializeField] private GameObject thismanManagerPrefab;
        [SerializeField] private GameObject fireworkPrefab;

        [Header("Timer")]
        [SerializeField] private float maxRemainedTime;
        [SerializeField] private float remainedPlayTime;
        [SerializeField] private float horrorEffectTime;

        /** Actions **/
        public Action OnStageFailAction { get; set; }
        public Action OnStageClearAction { get; set; }

        public Action OnStageStartAction { get; set; }

        /** State Variables **/
        private bool isLoadingScene = false;

        public PlayerController Controller { get => controller; }

        /** GameObjects **/
        private GameObject thismanManager = null;
        private GameObject firework = null;

        private void InitScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneEnums.Game.ToString()) return;   // 게임씬에만 필요한 Init입니다.
            directionalLights.Clear();

            controller = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER).GetComponent<PlayerController>();

            var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
            foreach (GameObject prop in initProps)
            {
                if (prop.GetComponent<Door>() != null) doorToOpen = prop.GetComponent<Door>();
                if (prop.GetComponent<Chair>() != null) startChair = prop.GetComponent<Chair>();
                if (prop.GetComponent<Light>() != null) directionalLights.Add(prop.GetComponent<Light>());
                if (prop.GetComponent<TVController>() != null) tvController = prop.GetComponent<TVController>();
                if (prop.GetComponent<BloodyFloorController>() != null) floorController = prop.GetComponent<BloodyFloorController>();
            }

            DeskManager.Instance.LoadDesks();
            SoundManager.Instance.ReleaseSound();

            remainedPlayTime = maxRemainedTime;
            isTimerSet = true;
        }

        private void ClearScene(Scene scene)
        {
            if (scene.name != SceneEnums.Game.ToString()) return;
            OnStageClearAction = null;
            OnStageFailAction = null;
        }


        // 클리어 했을 때 불러오는 코루틴
        // 폭죽이 터지며 플레이어가 회전함.
        private IEnumerator LoadSceneAfterClear(SceneEnums sceneEnum)
        {
            isLoadingScene = true;

            firework = Instantiate(fireworkPrefab,
                controller.transform.position + controller.transform.forward, Quaternion.identity);
            firework.GetComponent<ParticleSystem>().Simulate(1f, true, true, false);
            firework.GetComponent<ParticleSystem>().Play();
            SoundManager.Instance.CreateAudioSource(firework.transform.position, SfxClipTypes.Firework, 1.0f);
            yield return new WaitForSeconds(0.3f);
            OnStageClearAction.Invoke();

            FinThismanManager();
            yield return new WaitForSeconds(1.0f);

            OnStageStartAction.Invoke();
            isLoadingScene = false;
        }

        // 스테이지 실패후 불러오는 코루틴
        // 경비 디스맨이 들어오는 로직 포함, 이를 수정해야 함.
        private IEnumerator LoadSceneAfterFail(SceneEnums sceneEnum)
        {
            isLoadingScene = true;

            // 경비 디스맨의 경우.
            // 디스맨 마다 각기 다른 로직이 필요합니다. Enum을 함수의 인자로 입력 받아서 분리를 하던.
            // 부모 디스맨 클래스를 받고 다형성을 이용하던 해야 합니다.
            {
                // 문열고 기다렸다가 Input Block, Spawn Thisman
                doorToOpen.Interact(controller);
                yield return new WaitForSeconds(0.8f);

                SpawnBouncerThisman();
                OnStageFailAction.Invoke();
                FinThismanManager();
                yield return new WaitForSeconds(0.8f);
            }

            // 씬 전환
            yield return StartCoroutine(screenBlocker.FadeInCoroutine(1.0f));
            UnityEngine.AsyncOperation async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneEnums), sceneEnum));
            yield return async;
            yield return StartCoroutine(screenBlocker.FadeOutCoroutine(0.5f));

            OnStageStartAction.Invoke();
            isLoadingScene = false;
        }

        // 경비 디스맨을 소환하는 로직입니다.
        private void SpawnBouncerThisman()
        {
            GameObject tmpThis = Instantiate(thismanPrefab, doorToOpen.OriginalPosition, Quaternion.identity);
            tmpThis.GetComponent<ThismanController>().SetThismanTarget(controller.transform);

            controller.GetComponent<PlayerController>().thismanState.Thisman = tmpThis.transform;

        }

        // 경비 디스맨 매니저를 추가합니다.
        private void InitThismanManager()
        {
            thismanManager = Instantiate(thismanManagerPrefab, transform);
            thismanManager.GetComponent<ThismanManager>().Init();
        }


        // 경비 디스맨 매니저를 삭제.
        private void FinThismanManager()
        {
            if (thismanManager != null)
            {
                Destroy(thismanManager); thismanManager = null;
            }
        }



        private bool isTimerSet = false;
        public bool IsTimerSet {  get { return isTimerSet; }  }

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
            if (stageClearConditions[currentStage]() && isTimerSet)
            {
                isTimerSet = false;
                OnStageClear(currentStage);
            }

        }

        public void IncreaseThismanProb()
        {
            thismanManager.GetComponent<ThismanManager>().IncreaseProb();
        }

        public void SetLightIntensity(float intensity)
        {
            foreach (var light in directionalLights)
            {
                light.intensity = intensity;
            }
        }

        public void TurnOnOffTV(bool turn)
        {
            tvController.OnOffTV(turn);
        }
    }
}