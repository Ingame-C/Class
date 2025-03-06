using Class.StateMachine;
using UnityEngine;

namespace Class
{

    public class Chair : Usable
    {
        private PropTypes proptype = PropTypes.Chair;
        public override PropTypes PropType { get => proptype; }


        protected override void Init()
        {

        }

        public override void Interact(PlayerController controller)
        {
            if (controller.StateMachine.CurrentState is not SitState)
                controller.StateMachine.ChangeState(controller.sitState);
        }
    }

}