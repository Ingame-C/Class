using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class Crayons : Grabbable, IInteractable
    {
        private PropTypes proptype = PropTypes.Crayons;
        public override PropTypes PropType { get => proptype; }

        protected override void Init()
        {

        }

    }
}