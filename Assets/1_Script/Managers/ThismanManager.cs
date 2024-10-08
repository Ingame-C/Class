using System;
using UnityEngine;

namespace Class
{

    public class ThismanManager : MonoBehaviour
    {

        #region Singleton
        private static ThismanManager instance;
        public static ThismanManager Instance { get { return instance; } }


        private void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@ThismanManager");
                if (go == null)
                {
                    go = new GameObject { name = "@ThismanManager" };
                    go.AddComponent<ThismanManager>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<ThismanManager>();

            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        void Awake()
        {
            Init();
        }

        #endregion

        [Header("GameObjects")]
        [SerializeField] private GameObject player;

        // 플레이어 시점고정, 등..

        private Vector3 spawnPosition;




    }
}
