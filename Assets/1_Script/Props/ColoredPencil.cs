using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class
{
    public class ColoredPencil : Grabbable, IInteractable
    {
        private PropTypes proptype = PropTypes.ColoredPencil;
        public override PropTypes PropType { get => proptype; }

        protected override void Init()
        {

        }

    }
}