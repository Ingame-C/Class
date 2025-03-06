using UnityEngine;

namespace Class.Manager
{
    /// <summary>
    /// 게임의 모든 매니저들을 통합 관리하는 클래스입니다.
    /// 리소스와 데이터 매니저를 포함한 모든 매니저의 초기화와 접근을 담당합니다.
    /// </summary>
    public class Managers : MonoBehaviour
    {
        #region Singleton
        private static Managers s_instance;
        public static Managers Instance { get { return s_instance; } }
        #endregion

        #region Private Fields
        private ResourceManager _resource = new ResourceManager();
        private DataManager _data = new DataManager();
        #endregion

        #region Public Properties
        /// <summary>
        /// 리소스 매니저에 접근할 수 있는 프로퍼티입니다.
        /// </summary>
        public static ResourceManager Resource { get { return Instance._resource; } }

        /// <summary>
        /// 데이터 매니저에 접근할 수 있는 프로퍼티입니다.
        /// </summary>
        public static DataManager Data { get { return Instance._data; } }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            InitializeManager();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 매니저를 초기화합니다.
        /// </summary>
        private void InitializeManager()
        {
            InitializeSingleton();
            InitializeSubManagers();
        }

        /// <summary>
        /// 싱글톤 인스턴스를 초기화합니다.
        /// </summary>
        private void InitializeSingleton()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        /// <summary>
        /// 하위 매니저들을 초기화합니다.
        /// </summary>
        private void InitializeSubManagers()
        {
            _resource.Init();
            _data.Init();
        }
        #endregion
    }
}
