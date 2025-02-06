using System.Collections;
using System.Collections.Generic;
using Class.UI;
using UnityEngine;

namespace Class
{
    public class OMRGrabbable : Grabbable
    {
        
        private PropTypes proptype = PropTypes.OMRCard;
        public override PropTypes PropType { get => proptype; }
        
        protected override void Init()
        {

        }
        
    }
}