using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class Replica : Grabbable, IInteractable
    {
        private PropTypes proptype = PropTypes.Replica;
        public override PropTypes PropType { get => proptype; }

        protected override void Init()
        {

        }

    }
}