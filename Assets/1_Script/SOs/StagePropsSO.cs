using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    [System.Serializable]
    public class Pair
    {
        
        public Vector3 Pos = new Vector3(0f, 0f, 0f);
        public Vector3 Rot = new Vector3(0f, 0f, 0f);
    }

    [System.Serializable]
    public class StagePropData {
        public string name;
        public GameObject prefab;
        public List<Pair> transforms = new List<Pair>();
    }

    /// <summary>
    /// 각 스테이지에서 로드해야할 정보들을 저장합니다.
    /// </summary>
    [CreateAssetMenu(fileName ="StageData", menuName ="Class/StageData")]
    public class StagePropsSO : ScriptableObject
    {
        public List<StagePropData> propDatas = new List<StagePropData>();
    }
}