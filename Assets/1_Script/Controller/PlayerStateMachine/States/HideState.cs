using UnityEngine;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 숨기 상태를 관리합니다.
    /// </summary>
    public class HideState : StateBase
    {
        #region Variables
        private Hidable currentHidable;
        private bool isESCPressed;
        #endregion

        #region Constructor
        public HideState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {
        }
        #endregion

        #region State Methods
        public override void Enter()
        {
            base.Enter();
            InitializeHideState();
        }

        public override void Exit()
        {
            base.Exit();
            CleanupHideState();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            GetInteractOutInput(out isESCPressed);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            HandleEscapeInput();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        #endregion

        #region Private Methods
        private void InitializeHideState()
        {
            controller.IsHiding = true;
            currentHidable = (Hidable)controller.RecentlyDetectedProp;

            if (controller.IsGrabbing)
            {
                controller.InteractableGrabbing.ReleaseObject();
            }

            SetPlayerPositionAndRotation();
            SetColliderTrigger(true);
        }

        private void CleanupHideState()
        {
            controller.IsHiding = false;
            controller.SetPlayerPosition(currentHidable.ReturnPosition);
            SetColliderTrigger(false);
        }

        private void HandleEscapeInput()
        {
            if (isESCPressed)
            {
                stateMachine.ChangeState(controller.idleState);
                isESCPressed = false;
            }
        }

        private void SetPlayerPositionAndRotation()
        {
            controller.SetPlayerPosition(currentHidable.HidePosition);
            controller.SetPlayerRotation(currentHidable.HideRotation);
        }

        private void SetColliderTrigger(bool isTrigger)
        {
            currentHidable.GetComponent<Collider>().isTrigger = isTrigger;
        }
        #endregion
    }
}