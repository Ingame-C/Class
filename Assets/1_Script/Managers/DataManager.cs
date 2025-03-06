using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Class.Manager
{
    /// <summary>
    /// 게임의 설정 데이터를 저장하는 클래스입니다.
    /// 소리 크기, 해상도 등의 설정을 관리합니다.
    /// </summary>
    [Serializable]
    public class SettingData
    {
        /// <summary>
        /// 기본 생성자입니다. 기본값으로 초기화합니다.
        /// </summary>
        public SettingData()
        {
            sfxVolume = 1f;
            bgmVolume = 1f;
        }

        public float sfxVolume;
        public float bgmVolume;

        // etc...
    }

    /// <summary>
    /// 게임 플레이 데이터를 저장하는 클래스입니다.
    /// 스테이지 클리어 상태 등의 게임 진행 상황을 관리합니다.
    /// </summary>
    [Serializable]
    public class GameplayData
    {
        /// <summary>
        /// 각 스테이지의 클리어 상태를 저장하는 리스트입니다.
        /// </summary>
        public List<bool> stageCleared = new List<bool>();
    }

    /// <summary>
    /// 게임의 데이터를 관리하는 매니저 클래스입니다.
    /// 설정 데이터와 게임 플레이 데이터의 저장 및 로드를 담당합니다.
    /// </summary>
    public class DataManager
    {
        /*
         * persistentDataPath : C:/Users/[UserName]/AppData/LocalLow/DefaultCompany/Class
         * 
         * 1. DataManager는 데이터를 저장/로드하기 위한 매니저입니다.
         * 2. 이 경로는 읽고 쓰기 가능한 경로입니다.
         * 3. 해당 데이터들은 git이 추적하지 않으므로 각자 관리하셔야 합니다. (오류나면 경로안에 있는 데이터 지우시면 됩니다)
         * 4. 위에 추가적인 정보저장을 위해 스크립트를 만든다면, 꼭 디폴트 생성자에서 변수 초기화해주세요.
         * 
         */

        #region Constants
        private const string SETTING_DATA_FILE = "SettingData.json";
        private const string PLAY_DATA_FILE = "PlayData.json";
        #endregion

        #region Private Fields
        private string settingDataPath;
        private string playDataPath;
        private SettingData settingData = null;
        private GameplayData gameplayData = null;
        #endregion

        #region Public Methods
        /// <summary>
        /// 데이터 매니저를 초기화합니다.
        /// 저장 경로를 설정하고 데이터를 로드합니다.
        /// </summary>
        public void Init()
        {
            InitializePaths();
            LoadAll();
        }

        /// <summary>
        /// 게임 시작 시 필요한 초기화를 수행합니다.
        /// </summary>
        public void OnStart()
        {
            // 게임 시작 시 필요한 초기화 로직
        }

        /// <summary>
        /// 모든 데이터를 저장합니다.
        /// </summary>
        public void SaveAll()
        {
            SaveData(settingData, settingDataPath);
            SaveData(gameplayData, playDataPath);
        }

        /// <summary>
        /// 특정 스테이지의 클리어 상태를 저장합니다.
        /// </summary>
        /// <param name="stageId">클리어한 스테이지의 ID</param>
        public void SaveClearStage(int stageId)
        {
            if (gameplayData.stageCleared.Count <= stageId)
            {
                gameplayData.stageCleared.Add(true);
            }
            else
            {
                gameplayData.stageCleared[stageId] = true;
            }
            Debug.Log($"Stage: {stageId} Clear!");
            SaveAll();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 데이터 파일의 경로를 초기화합니다.
        /// </summary>
        private void InitializePaths()
        {
            string basePath = Application.persistentDataPath;
            settingDataPath = Path.Combine(basePath, SETTING_DATA_FILE);
            playDataPath = Path.Combine(basePath, PLAY_DATA_FILE);
        }

        /// <summary>
        /// 모든 데이터를 로드합니다.
        /// </summary>
        private void LoadAll()
        {
            LoadData(ref settingData, settingDataPath);
            LoadData(ref gameplayData, playDataPath);
        }

        /// <summary>
        /// 특정 데이터를 저장합니다.
        /// </summary>
        /// <typeparam name="T">저장할 데이터의 타입</typeparam>
        /// <param name="data">저장할 데이터</param>
        /// <param name="path">저장할 파일 경로</param>
        private void SaveData<T>(T data, string path)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// 특정 데이터를 로드합니다.
        /// </summary>
        /// <typeparam name="T">로드할 데이터의 타입</typeparam>
        /// <param name="data">로드된 데이터를 저장할 변수</param>
        /// <param name="path">로드할 파일 경로</param>
        private void LoadData<T>(ref T data, string path) where T : new()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<T>(json);
            }
            else
            {
                data = new T();
                Debug.Log($"Created new data file at: {path}");
            }
        }
        #endregion
    }
}
