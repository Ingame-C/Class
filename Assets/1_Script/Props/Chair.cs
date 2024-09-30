using UnityEngine;

namespace Class
{

    public class Chair : PropsBase, IInteractable
    {

        protected override void Init()
        {
            propType = PropTypes.Chair;
        }

    }

}