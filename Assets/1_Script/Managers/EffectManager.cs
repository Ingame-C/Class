using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Class
{
    // Horror Effect를 비롯한 Effect들을 다루기 위한 매니저 입니다.
    // TODO: Effect들을 원하는 시간과 공간에 적절하게 발동시킬 수 있어야 함.

    public class EffectManager : MonoBehaviour
    {
        #region 싱글톤 패턴

        private static EffectManager instance;
        public static EffectManager Instance { get { return instance; } }


        private void Init()
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("@EffectManager");
                if (go == null)
                {
                    go = new GameObject { name = "@EffectManager" };
                    go.AddComponent<EffectManager>();
                }

                DontDestroyOnLoad(go);
                instance = go.GetComponent<EffectManager>();

            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }

        #endregion


        // TODO: 이펙트의 발동을 다루는 로직을 만들어야 함.



    }

}

