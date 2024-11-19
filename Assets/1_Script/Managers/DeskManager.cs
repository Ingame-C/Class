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

            // HACK : 테스트용 케이스입니다. 지워도 상관없습니다.
            preset.Add(new List<PropTypes>{
                 PropTypes.Crayons, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
                PropTypes.ColoredPencil, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
                PropTypes.Pallet, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
                PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None, PropTypes.None,
            });

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
                if (prop.name == "Tables")
                {
                    go = prop;
                    break;
                }
            }

            if(go == null)
            {
                Debug.LogError("There is no 'Tables' object in Scene");
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



    }
}
