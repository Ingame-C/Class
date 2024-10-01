using UnityEngine;

namespace Class
{

    public class Chair : PropsBase, IInteractable
    {
        private PropTypes proptype = PropTypes.Chair;
        public override PropTypes PropType { get => proptype; }


        protected override void Init()
        {

        }

    }

}