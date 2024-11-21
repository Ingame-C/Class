using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class{
    public class Chalk : Usable
    {

        [SerializeField] private GameObject hintScreen;
        private PropTypes proptype = PropTypes.Chalk;
        
        public override PropTypes PropType => proptype;

        public override void Interact(PlayerController controller)
        {
            var hintScreenAnimator = hintScreen.GetComponent<Animator>();
            hintScreenAnimator.SetBool(Constants.FLAG_LOCKERDOOR, true);
        }
    }
}