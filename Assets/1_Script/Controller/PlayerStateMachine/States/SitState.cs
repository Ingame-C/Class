using UnityEditor;
using UnityEngine;


namespace Class.StateMachine
{
    public class SitState : StateBase
    {
        private PropsBase chair;
        private Vector3 prevPosition;

        public SitState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            controller.IsInteracting = true;
            SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Sweep);
            chair = controller.RecentlyDetectedProp;
            chair.GetComponent<BoxCollider>().isTrigger = true;

            prevPosition = controller.transform.position;
            controller.SetPlayerPosition(chair.transform.position);
            controller.SetPlayerRotation(chair.transform.rotation);
        }

        public override void Exit()
        {
            base.Exit();
            controller.IsInteracting = false;
            controller.SetPlayerPosition(prevPosition);
            chair.GetComponent<BoxCollider>().isTrigger = false;
        }


        public override void HandleInput()
        {
            base.HandleInput();

            GetInteractOutInput(out isESCPressed);
            GetMouseInput(out mouseX, out mouseY);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(isESCPressed)
            {
                stateMachine.ChangeState(controller.idleState);
                isESCPressed = false;
            }

            controller.RotateWithMouse(mouseX, mouseY);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.RaycastInteractableObject();
        }
    }

}
