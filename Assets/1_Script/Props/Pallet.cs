using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class Pallet : Grabbable, IInteractable
    {
        private PropTypes proptype = PropTypes.Pallet;
        public override PropTypes PropType { get => proptype; }

        protected override void Init()
        {

        }

    }
}