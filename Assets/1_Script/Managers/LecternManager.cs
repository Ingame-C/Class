using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Class
{
    public class LecternManager : MonoBehaviour
    {
        private Lectern lectern;
        private static LecternManager instance;
        public static LecternManager Instance { get { return instance; } }

        private void Init()
        {
            #region Singleton
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
            #endregion

            lectern = GameObject.Find("Counter_01")?.GetComponent<Lectern>();
            if (lectern == null)
            {
                Debug.Log("Lectern is not found");
            }

        }

        public void Start()
        {
            Init();
        }

        public bool CheckCleared()
        {
            //TODO: 클리어 조건을 달성했는 지의 여부를 확인하는 로직이 필요합니다.
            return true;
        }

    }
}
