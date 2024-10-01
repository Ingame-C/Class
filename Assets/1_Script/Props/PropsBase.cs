
using UnityEngine;

namespace Class
{

    public abstract class PropsBase : MonoBehaviour
    {

        // 각 자식클래스가 정의하는것을 강제하도록 추상 프로퍼티 사용
        public abstract PropTypes PropType { get; }  

        protected virtual void Init()
        {

        }

        private void Awake()
        {
            Init();
        }

    }

}