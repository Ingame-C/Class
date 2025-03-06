using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        #endregion

        #region Private Fields
        private Lectern lectern;
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeManager();
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
            return false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 매니저를 초기화합니다.
        /// </summary>
        private void InitializeManager()
        {
            InitializeSingleton();
            InitializeLectern();
        }

        /// <summary>
        /// 싱글톤 인스턴스를 초기화합니다.
        /// </summary>
        private void InitializeSingleton()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        /// <summary>
        /// 강의대 컴포넌트를 초기화합니다.
        /// </summary>
        private void InitializeLectern()
        {
            lectern = GameObject.Find("Counter_01")?.GetComponent<Lectern>();
            if (lectern == null)
            {
                Debug.LogWarning("Lectern is not found");
            }
        }
        #endregion
    }
}
