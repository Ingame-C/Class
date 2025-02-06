using System.Collections;
using System.Collections.Generic;
using Class.UI;
using UnityEngine;

namespace Class
{
    public class OMRUsable : Usable
    {
        
        [SerializeField] private UI.UI omrCard;
        
        private PropTypes proptype = PropTypes.OMRCard;
        public override PropTypes PropType { get => proptype; }
        
        protected override void Init()
        {

        }

        public override void Interact(PlayerController controller)
        {
            if (controller.InteractableGrabbing is not ComputerPen) return;
            controller.CurrentUI = omrCard;
        }
    }
}