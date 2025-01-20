using System;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;

namespace Class.Manager
{
    [Serializable]
    public class StageInfos
    {
        public StageInfo[] stageInfo;
    }

    [Serializable]
    public class StageInfo
    {
        public int stageId;
        public int difficultyLevel;
        
        // TODO : 스테이지에 따라 다른 데이터를 추가해야합니다.
        // ex. 스테이지를 다른 씬에 만듦    -> 각 씬 이름이 필요
        // ex. 스테이지를 같은 씬에서 만듦  -> 활성화/비활성화 할 props list, 추가적인 이벤트에 대한 정보 필요 
    }

    public class ResourceManager
    {
        /** Json file Paths**/ 
        private string stageInfoPath = "JsonData/StageData";
        private string stagePropSOPath = "ScriptableObject/StageData/";

        /** Data Containers **/
        private StageInfos stageInfos = new StageInfos();

        private StagePropsSO[] stagePropsSOs;

        public void Init()
        {
            stageInfos = JsonUtility.FromJson<StageInfos>(Resources.Load<TextAsset>(stageInfoPath).text);
            stagePropsSOs = Resources.LoadAll<StagePropsSO>(stagePropSOPath);
        }

        /** Getter Functions **/
        public StageInfo GetStageInfo(int id)
        {
            var ret = from info in stageInfos.stageInfo
                      where info.stageId == id
                      select info;
            if (ret.Count() != 1) Debug.LogWarning("Stage Info Err : Check Ids in json file");

            return ret.First();
        }

        public int GetStageCount()
        {
            return stageInfos.stageInfo.Length;
        }
        
        public StagePropsSO GetStagePropsSO(int id)
        {
            if (stagePropsSOs.Count() <= id)
            {
                Debug.LogError("Out Of Index : Insufficient StagePropSOs.");
                return null;
            }
            return stagePropsSOs[id];
        }

    }

}