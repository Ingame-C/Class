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
                GameObject go = GameObject.Find("@GameManagerEx");
                if (go == null)
                {
                    go = new GameObject { name = "@GameManagerEx" };
                    go.AddComponent<GameManagerEx>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<GameManagerEx>();

            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        #endregion


        private void Awake()
        {
            Init();
        }

        public void OnStageClear(int clearStageId)
        {

        }

        public void OnStageFailed(int faildStageId)
        {

        }

        private void MoveStage(int stageId)
        {

        }




        [Header("Load Scene")]
        [SerializeField] ScreenBlocker screenBlocker;

        private bool isLoadingScene = false;


        public void LoadScene(SceneEnums sceneEnum)
        {
            if (!isLoadingScene) StartCoroutine(LoadSceneCoroutine(sceneEnum));
        }

        private IEnumerator LoadSceneCoroutine(SceneEnums sceneEnum)
        {
            isLoadingScene = true;

            // TODO : 씬 로드 전/후로 처리 넣어야합니다. (ex. Breath sound, Screen Fade In/Out, Clear/Set stages)
            
            yield return StartCoroutine(screenBlocker.FadeInCoroutine(1.0f));
            AsyncOperation async = SceneManager.LoadSceneAsync(Enum.GetName(typeof(SceneEnums), sceneEnum));
            yield return async;
            yield return StartCoroutine(screenBlocker.FadeOutCoroutine(0.5f));

            isLoadingScene = false;
        }

        // HACK : 테스트 코드입니다.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                LoadScene(SceneEnums.Game);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                LoadScene(SceneEnums.Test);
            }
        }

    }
}