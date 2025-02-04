using UnityEngine;

namespace Class.StateMachine
{
    public class IdleState : StateBase
    {

        public IdleState(PlayerController controller, PlayerStateMachine stateMachine) 
            : base(controller, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            vertInputRaw = horzInputRaw = 0f;
            
        }

        public override void Exit()
        {
            base.Exit();
        }


        public override void HandleInput()
        {
            base.HandleInput();

            GetInteractOutInput(out isESCPressed);
            GetMovementInputRaw(out vertInputRaw, out horzInputRaw);
            GetMouseInput(out mouseX, out mouseY);
            GetInteractableInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Mathf.Abs(vertInputRaw) >= 0.9f || Mathf.Abs(horzInputRaw) >= 0.9f) 
            {
                stateMachine.ChangeState(controller.walkState);
            }


            if (isESCPressed && controller.UIisSet)
            {
                controller.CurrentUI = null;
                isESCPressed = false;
            }
            
            if (!controller.UIisSet)
            {
                controller.RotateWithMouse(mouseX, mouseY);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.RaycastInteractableObject();
        }
    }
}
