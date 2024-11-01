using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Class
{
    public class DeskManager: MonoBehaviour
    {

        [Header("Desks")]
        [SerializeField] private Desk[] Desks;

        private List<List<PropTypes>> preset = new List<List<PropTypes>>();
        private int presetIndex;

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

        public bool CheckCleared()
        {
            for(int i = 0; i < 20; i++) {
                if(preset[presetIndex][i] == PropTypes.None && Desks[i].props.Count == 0)
                {
                    continue;
                }
                if (Desks[i].props.Any(prop => prop != preset[presetIndex][i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void SetRandomPreset()
        {
            presetIndex = UnityEngine.Random.Range(0, 5);
        }



    }
}
