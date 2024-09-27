using System.Threading;
using UnityEngine;

namespace Class.StateMachine
{

    public class WalkState : StateBase
    {

        private float diagW;
        private float speed;

        public WalkState(PlayerController controller, PlayerStateMachine stateMachine) 
            : base(controller, stateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();

            vertInput = horzInput = vertInputRaw = horzInputRaw = 0f;
            diagW = 1.0f;

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();

            GetMovementInput(out vertInput, out horzInput);
            GetMovementInputRaw(out vertInputRaw, out horzInputRaw);
            GetMouseInput(out mouseX, out mouseY);
            GetInteractableInput();
        }


        public override void LogicUpdate()
        {
            base.LogicUpdate();

            speed = Mathf.Abs(vertInput) + Mathf.Abs(horzInput);

            if (Mathf.Approximately(speed, 0f))
            {
                stateMachine.ChangeState(controller.idleState);
            }

            controller.RotateWithMouse(mouseX, mouseY);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.RaycastInteractableObject();

            diagW = (Mathf.Abs(horzInput) > 0.5f && Mathf.Abs(vertInput) > 0.5f) ? 0.71f : 1.0f;
            controller.WalkWithArrow(horzInputRaw, vertInputRaw, diagW);
        }
    }

}
