
using UnityEngine;

namespace Class
{

    public class PropsBase : MonoBehaviour
    {
        protected PropTypes propType = PropTypes.None;
        public PropTypes PropType { get { return propType; } }  

        protected virtual void Init()
        {

        }

        private void Awake()
        {
            Init();
        }

    }

}