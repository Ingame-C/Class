using UnityEngine;

namespace Class.Manager {

    public class Managers : MonoBehaviour
    {
        #region Singleton
        static Managers s_instance;
        public static Managers Instance { get { return s_instance; } }

        public void Init()
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

            _resource.Init();
            _data.Init();
        }

        #endregion


        private ResourceManager _resource = new ResourceManager();
        private DataManager _data = new DataManager();

        public static ResourceManager Resource { get { return Instance._resource; } }
        public static DataManager Data { get { return Instance._data; } }

        private void Awake()
        {
            Init();
        }

    }
}
