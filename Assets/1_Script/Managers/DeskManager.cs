using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Class
{
    public class DeskManager: MonoBehaviour
    {

        [Header("Desks")]
        [SerializeField] private Desk[] Desks;

        private List<List<PropTypes>> preset = new List<List<PropTypes>>();
        [SerializeField] private int presetIndex;
        public int PresetIndex { get { return presetIndex; } }

        [Header("Logics")]
        [SerializeField] private float heightOfReflectOnly = 1.0f;

        [Header("Prefabs")]
        [Space]
        [SerializeField] private GameObject ReflectOnlyCrayon;
        [SerializeField] private GameObject ReflectOnlyColoredPen;
        [SerializeField] private GameObject ReflectOnlyPallet;


        [Header("Parents")]
        [Space]
        [SerializeField] private Transform ReflectOnlyParent;

        #region Singleton
        private static DeskManager instance;
        public static DeskManager Instance { get { return instance; } }


        private void Init()
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
        #endregion

        void Awake()
        {
            Init();
            #region initialization preset
            preset.Add(new List<PropTypes>
            {
                PropTypes.None, PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.ColoredPencil,     // 1분단
                PropTypes.Pallet, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,      // 2분단
                PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.Pallet, PropTypes.Crayons,         // 3분단
                PropTypes.None, PropTypes.Pallet, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None       // 4분단
            });

            preset.Add(new List<PropTypes>
            {
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.Pallet, PropTypes.None, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.Pallet, PropTypes.Crayons, PropTypes.None,
                PropTypes.Crayons, PropTypes.Pallet, PropTypes.None, PropTypes.None, PropTypes.ColoredPencil,
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.Crayons,
            });

            preset.Add(new List<PropTypes>
            {
                PropTypes.ColoredPencil, PropTypes.Crayons, PropTypes.Pallet, PropTypes.None, PropTypes.None,
                PropTypes.Crayons, PropTypes.Pallet, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,
                PropTypes.Pallet, PropTypes.ColoredPencil, PropTypes.Crayons, PropTypes.None, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
            });

            preset.Add(new List<PropTypes>
            {
                PropTypes.None, PropTypes.Pallet, PropTypes.Pallet, PropTypes.None, PropTypes.Crayons,
                PropTypes.None, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,
                PropTypes.None, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.Pallet,
                PropTypes.Crayons, PropTypes.None, PropTypes.Crayons, PropTypes.None, PropTypes.ColoredPencil,
            });

            preset.Add(new List<PropTypes>
            {
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.Pallet, PropTypes.None,
                PropTypes.None, PropTypes.Crayons, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None,
                PropTypes.Crayons, PropTypes.None, PropTypes.Pallet, PropTypes.None, PropTypes.Crayons,
                PropTypes.Pallet, PropTypes.None, PropTypes.ColoredPencil, PropTypes.None, PropTypes.None,
            });
            #endregion
        }

        /// <summary>
        /// Load Desks after loading scene
        /// </summary>
        public void LoadDesks()
        {
            GameObject go = null;
            var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
            foreach (GameObject prop in initProps)
            {
                if (prop.name == "Desks")
                {
                    go = prop;
                    break;
                }
            }

            if(go == null)
            {
                Debug.LogError("There is no 'Desks' object in Scene");
                return;
            }

            int counter = 0;
            foreach (Transform child in go.transform)
            {
                if(counter >= Desks.Count() || child.GetComponent<Desk>() == null)
                {
                    Debug.LogError("Desk Binding Err : count mismatch or child doesn't have Desk");
                    return;
                }
                Desks[counter++] = child.GetComponent<Desk>();
            }
        }

        public bool CheckCleared()
        {
            for(int i = 0; i < 20; i++) {
                if(preset[presetIndex][i] == PropTypes.None && Desks[i].props.Count == 0)
                {
                    continue;
                }
                if (!Desks[i].props.Any(prop => prop == preset[presetIndex][i]))
                {
                    // Debug.Log("Wrong");
                    return false;
                }
            }
            // Debug.Log("Correct");
            return true;
        }

        public void SetRandomPreset()
        {
            presetIndex = UnityEngine.Random.Range(0, preset.Count());
        }

        public void GenerateReflectionOnly()
        {
            List<GameObject> ReflectionOnlys = new List<GameObject>();
            for (int i = 0; i < Desks.Length; i++)
            {
                if (preset[presetIndex][i] == PropTypes.Crayons)
                {
                    var obj = Instantiate(ReflectOnlyCrayon);
                    obj.transform.position = Desks[i].transform.position + Vector3.up * heightOfReflectOnly;
                    ReflectionOnlys.Add(obj);
                }
                else if(preset[presetIndex][i] == PropTypes.ColoredPencil)
                {
                    var obj = Instantiate(ReflectOnlyColoredPen);
                    obj.transform.position = Desks[i].transform.position + Vector3.up * heightOfReflectOnly;
                    ReflectionOnlys.Add(obj);
                }
                else if (preset[presetIndex][i] == PropTypes.Pallet)
                {
                    var obj = Instantiate(ReflectOnlyPallet);
                    obj.transform.position = Desks[i].transform.position + Vector3.up * heightOfReflectOnly;
                    ReflectionOnlys.Add(obj);
                }
            }


            ReflectionOnlys.ForEach(i => i.transform.SetParent(ReflectOnlyParent));


        }
    }
}
