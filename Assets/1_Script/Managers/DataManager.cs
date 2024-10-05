using System;
using System.IO;
using UnityEngine;

namespace Class.Manager
{

    [Serializable]
    public class SettingData {     // Setting 저장 (소리 크기, 해상도..)

        public SettingData()
        {
            sfxVolume = 1f;
            bgmVolume = 1f;
        }

        public float sfxVolume;
        public float bgmVolume;

        // etc...
    }

    [Serializable]
    public class GameplayData {     // 유저가 플레이한 게임 데이터를 저장
    
    }


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

        /** Paths **/
        private string settingDataPath;
        private string playDataPath;

        /** Datas **/
        private SettingData settingData = null;
        private GameplayData gameplayData = null;

        public void Init()
        {
            settingDataPath = Application.persistentDataPath + "/SettingData.json";
            playDataPath = Application.persistentDataPath + "/PlayData.json";

            LoadAll();
        }


        /** Save/Load Functions **/
        private void LoadAll()
        {
            LoadData<SettingData>(ref settingData, settingDataPath);
            LoadData<GameplayData>(ref gameplayData, playDataPath);
        }
        public void SaveAll()
        {
            SaveData<SettingData>(ref settingData, settingDataPath);
            SaveData<GameplayData>(ref gameplayData, playDataPath);
        }

        private void SaveData<T>(ref T data, string path)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
        private void LoadData<T>(ref T data, string path) where T : new()
        {
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<T>(json);
            }
            else
            {
                data = new T();

                Debug.Log(data.ToString());
            }
        }


    }

}
