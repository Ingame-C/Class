using UnityEngine;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 대기 상태를 관리합니다.
    /// </summary>
    public class IdleState : StateBase
    {
        #region Constants
        private const float MOVEMENT_THRESHOLD = 0.9f;
        private int idleHash;
        #endregion

        #region Constructor
        public IdleState(PlayerController controller, PlayerStateMachine stateMachine, Animator animator)
            : base(controller, stateMachine, animator)
        {
            idleHash = Animator.StringToHash("Idle");
        }
        #endregion

        #region State Methods
        public override void Enter()
        {
            base.Enter();
            ResetMovementInput();
            animator.CrossFade(idleHash, 0.1f);
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
            HandleStateTransitions();
            HandleUIState();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.RaycastInteractableObject();
        }
        #endregion

        #region Private Methods
        private void ResetMovementInput()
        {
            vertInputRaw = horzInputRaw = 0f;
        }

        private void HandleStateTransitions()
        {
            if (ShouldChangeToWalkState())
            {
                stateMachine.ChangeState(controller.walkState);
            }
        }

        private bool ShouldChangeToWalkState()
        {
            return Mathf.Abs(vertInputRaw) >= MOVEMENT_THRESHOLD || 
                   Mathf.Abs(horzInputRaw) >= MOVEMENT_THRESHOLD;
        }

        private void HandleUIState()
        {
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
        #endregion
    }
}
