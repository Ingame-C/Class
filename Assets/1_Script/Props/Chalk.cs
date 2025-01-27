using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class{
    public class Chalk : Usable
    {

        [SerializeField] private HintController hintController;
        private PropTypes proptype = PropTypes.Chalk;
        
        public override PropTypes PropType => proptype;

        public override void Interact(PlayerController controller)
        {
            if (hintController != null)
            {
                hintController.SetHintAppear();
                this.gameObject.SetActive(false);
            }
        }
    }
}