using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);

                _resource.Init();
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
