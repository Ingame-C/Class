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

        #region Private Fields
        private Lectern lectern;
        private List<ToggleGroup> answers;
        private bool isClear = false;
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
        public bool CheckCleared()
        {
            // TODO: 클리어 조건을 달성했는 지의 여부를 확인하는 로직이 필요합니다.
            return isClear;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 매니저를 초기화합니다.
        /// </summary>
        private void InitializeManager()
        {
            InitializeLectern();
        }
        

        /// <summary>
        /// 강의대 컴포넌트를 초기화합니다.
        /// </summary>
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


        private bool isChecked = false;
        private void CheckClear()
        {
            for (int i = 0; i < answers.Count; i++)
            {
                foreach (var button in answers[i].GetComponentsInChildren<Toggle>())
                {
                    if (button.isOn)
                    {
                        isChecked = true;
                    }
                }
                if (!isChecked && i != 20) return;
                else if (i == 20)
                {
                    //TODO: Gameover
                    GameManagerEx.Instance.OnStageFailed(GameManagerEx.Instance.CurrentStage); // test 코드
                }
                isChecked = false;
            }
            isClear = true;
        }
        
        #endregion
    }
}
