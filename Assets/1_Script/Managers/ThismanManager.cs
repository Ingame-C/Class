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
        [SerializeField] private GameObject thismanPrefab;
        [SerializeField] private GameObject player;

        // 플레이어 시점고정, 등..
        public Action StageOverAction;


        private Vector3 spawnPosition;
        private void StageOver()
        {
            // TODO : 앞 문 위치로 설정해줘야됩니다. 
            spawnPosition = new Vector3(-6.5f, 1.5f, 6.4f);

            GameObject tmpThis = Instantiate(thismanPrefab, spawnPosition, Quaternion.identity);
            tmpThis.GetComponent<ThismanController>().SetThismanTarget(player.transform);

            player.GetComponent<PlayerController>().thismanState.Thisman = tmpThis.transform;

            StageOverAction.Invoke();
        }


        private void Update()
        {
            // HACK : 테스트 코드입니다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StageOver();
            }
        }
    }
}
