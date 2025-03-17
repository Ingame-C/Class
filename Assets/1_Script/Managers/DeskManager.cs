using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Class
{
    /// <summary>
    /// 책상과 관련된 모든 기능을 관리하는 매니저 클래스입니다.
    /// 책상의 프리셋 관리, 반영 전용 오브젝트 생성 등을 담당합니다.
    /// </summary>
    public class DeskManager : MonoBehaviour
    {
        #region Singleton
        private static DeskManager instance;
        public static DeskManager Instance { get { return instance; } }
        #endregion

        #region Serialized Fields
        [Header("Desks")]
        [SerializeField] private Desk[] Desks;

        [Header("Preset Settings")]
        [SerializeField] private int presetIndex;
        public int PresetIndex { get { return presetIndex; } }

        [Header("Reflection Settings")]
        [SerializeField] private float heightOfReflectOnly = 1.0f;

        [Header("Prefabs")]
        [Space]
        [SerializeField] private GameObject ReflectOnlyCrayon;
        [SerializeField] private GameObject ReflectOnlyColoredPen;
        [SerializeField] private GameObject ReflectOnlyPallet;


        [Header("Parents")]
        [Space]
        [SerializeField] private Transform ReflectOnlyParent;
        #endregion

        #region Private Fields
        private List<List<PropTypes>> preset = new List<List<PropTypes>>();
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeSingleton();
            InitializePresets();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 씬 로드 후 책상들을 로드합니다.
        /// </summary>
        public void LoadDesks()
        {
            GameObject desksParent = FindDesksParent();
            if (desksParent == null) return;

            LoadDeskComponents(desksParent);
        }

        /// <summary>
        /// 현재 책상 배치가 프리셋과 일치하는지 확인합니다.
        /// </summary>
        /// <returns>일치하면 true, 아니면 false</returns>
        public bool CheckCleared()
        {
            for(int i = 0; i < 20; i++)
            {
                if (!IsDeskCorrect(i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 랜덤한 프리셋을 선택합니다.
        /// </summary>
        public void SetRandomPreset()
        {
            presetIndex = UnityEngine.Random.Range(0, preset.Count());
        }

        /// <summary>
        /// 반영 전용 오브젝트들을 생성합니다.
        /// </summary>
        public void GenerateReflectionOnly()
        {
            List<GameObject> reflectionOnlys = CreateReflectionOnlyObjects();
            SetReflectionOnlysParent(reflectionOnlys);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 싱글톤 인스턴스를 초기화합니다.
        /// </summary>
        private void InitializeSingleton()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@DeskManager");
                if (go == null)
                {
                    go = new GameObject { name = "@DeskManager" };
                    go.AddComponent<DeskManager>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<DeskManager>();
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        /// <summary>
        /// 프리셋 데이터를 초기화합니다.
        /// </summary>
        private void InitializePresets()
        {
            InitializePreset1();
            InitializePreset2();
            InitializePreset3();
            InitializePreset4();
            InitializePreset5();
        }

        private void InitializePreset1()
        {
            preset.Add(new List<PropTypes>
            {
                PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.Pallet,   
                PropTypes.None, PropTypes.None, PropTypes.Pallet, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,     
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None,        
                PropTypes.None, PropTypes.Pallet, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None,
                PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None
            });
        }

        private void InitializePreset2()
        {
            preset.Add(new List<PropTypes>
            {
                PropTypes.Pallet, PropTypes.Crayons, PropTypes.Pallet, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,   
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,     
                PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.Pallet,        
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None
            });
        }

        private void InitializePreset3()
        {
            preset.Add(new List<PropTypes>
            {
                PropTypes.None, PropTypes.Pallet, PropTypes.None, PropTypes.None, PropTypes.Pallet, PropTypes.Crayons,   
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.None,     
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,        
                PropTypes.ColoredPencil, PropTypes.ColoredPencil, PropTypes.Pallet, PropTypes.Crayons, PropTypes.ColoredPencil, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None
            });
        }

        private void InitializePreset4()
        {
            preset.Add(new List<PropTypes>
            {
                PropTypes.None, PropTypes.Pallet, PropTypes.None, PropTypes.Crayons, PropTypes.Crayons, PropTypes.None,   
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.ColoredPencil, PropTypes.Pallet, PropTypes.None, PropTypes.None,     
                PropTypes.Pallet, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,        
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None
            });
        }

        private void InitializePreset5()
        {
            preset.Add(new List<PropTypes>
            {
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,   
                PropTypes.Pallet, PropTypes.Pallet, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,     
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Pallet,        
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Crayons, PropTypes.Crayons,
                PropTypes.Crayons, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None
            });
        }

        private GameObject FindDesksParent()
        {
            var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
            GameObject desksParent = initProps.FirstOrDefault(prop => prop.name == "Desks");

            if(desksParent == null)
            {
                Debug.LogError("There is no 'Desks' object in Scene");
                return null;
            }

            return desksParent;
        }

        private void LoadDeskComponents(GameObject desksParent)
        {
            int counter = 0;
            foreach (Transform child in desksParent.transform)
            {
                if(counter >= Desks.Length || child.GetComponent<Desk>() == null)
                {
                    Debug.LogError("Desk Binding Err : count mismatch or child doesn't have Desk");
                    return;
                }
                Desks[counter++] = child.GetComponent<Desk>();
            }
        }

        private bool IsDeskCorrect(int index)
        {
            if(preset[presetIndex][index] == PropTypes.None && Desks[index].props.Count == 0)
            {
                return true;
            }
            return Desks[index].props.Any(prop => prop == preset[presetIndex][index]);
        }

        private List<GameObject> CreateReflectionOnlyObjects()
        {
            List<GameObject> reflectionOnlys = new List<GameObject>();
            for (int i = 0; i < Desks.Count(); i++)
            {
                GameObject obj = CreateReflectionOnlyObject(i);
                if (obj != null)
                {
                    reflectionOnlys.Add(obj);
                }
            }
            return reflectionOnlys;
        }

        private GameObject CreateReflectionOnlyObject(int index)
        {
            GameObject prefab = GetReflectionOnlyPrefab(preset[presetIndex][index]);
            if (prefab == null) return null;

            GameObject obj = Instantiate(prefab);
            obj.transform.position = Desks[index].transform.position + Vector3.up * heightOfReflectOnly;
            return obj;
        }

        private GameObject GetReflectionOnlyPrefab(PropTypes propType)
        {
            switch (propType)
            {
                case PropTypes.Crayons:
                    return ReflectOnlyCrayon;
                case PropTypes.ColoredPencil:
                    return ReflectOnlyColoredPen;
                case PropTypes.Pallet:
                    return ReflectOnlyPallet;
                default:
                    return null;
            }
        }

        private void SetReflectionOnlysParent(List<GameObject> reflectionOnlys)
        {
            reflectionOnlys.ForEach(obj => obj.transform.SetParent(ReflectOnlyParent));
        }
        #endregion
    }
}
