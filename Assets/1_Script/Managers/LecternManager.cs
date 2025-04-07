using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SerializeField, Required] private GameObject omrCardUI;
        public bool IsClear { get; private set; }

        #region Private Fields

        private Lectern lectern;
        private List<ToggleGroup> answers;
        private bool isChecked = false;

        #endregion

        #region Unity Methods

        private void Start()
        {
            InitializeManager();
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
            for (int i = 0; i < answers.Count; i++)
            {
                foreach (var button in answers[i].GetComponentsInChildren<Toggle>())
                {
                    isChecked = button.isOn;
                    if (i == 20 && isChecked) return;
                }

                if (!isChecked) return;

                isChecked = false;
            }

            IsClear = true;
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

