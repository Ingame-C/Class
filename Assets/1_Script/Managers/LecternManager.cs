using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Class
{
    /// <summary>
    /// 강의대와 관련된 기능을 관리하는 매니저 클래스입니다.
    /// 강의대의 상태 관리와 클리어 조건 확인을 담당합니다.
    /// </summary>
    public class LecternManager : MonoBehaviour
    {

        
        #region Singleton
        private static LecternManager instance;
        public static LecternManager Instance { get { return instance; } }
        
        private void InitializeSingleton()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@LecternManager");
                if (go == null)
                {
                    go = new GameObject { name = "@LecternManager" };
                    go.AddComponent<LecternManager>();
                }
                instance = go.GetComponent<LecternManager>();
            }

        }
        
        #endregion
        
        [SerializeField, Required] private GameObject omrCardUI;
        public bool IsClear { get; private set; }
        public bool isAllChecked = false;
        
        #region Private Fields

        private Lectern lectern;
        private List<ToggleGroup> answers;
        public List<bool> isOnList;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            InitializeSingleton();
        }

        private void Start()
        {
            InitializeManager();
            isOnList = new List<bool>();
            for (int i = 0; i < 30; i++)
            {
                isOnList.Add(false);
            }
        }

        private void Update()
        {
            if (!lectern.Grabbable) return;
            CheckClear();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 강의대의 클리어 조건이 달성되었는지 확인합니다.
        /// </summary>
        /// <returns>클리어 조건이 달성되었으면 true, 아니면 false</returns>
        public void CheckClear()
        {
            if (!isToggleButtonsBeOn()) return;

            IsClear = true;
        }

        public bool isToggleButtonsBeOn()
        {
            for (int i = 0; i < 30; i++)
            {
                if (!isOnList[i] && i != 19)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Private Methods

        private void InitializeManager()
        {
            InitializeLectern();
        }


        private void InitializeLectern()
        {
            lectern = GameObject.Find("Counter_01")?.GetComponent<Lectern>();
            var answersList = omrCardUI.GetComponentsInChildren<ToggleGroup>();
            answers = answersList.ToList();

            if (lectern == null)
            {
                Debug.LogWarning("Lectern is not found");
            }
        }

        #endregion
    }
}

