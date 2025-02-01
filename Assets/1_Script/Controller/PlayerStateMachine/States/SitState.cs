using UnityEngine;


namespace Class.StateMachine
{
    public class SitState : StateBase
    {
        private PropsBase chair;
        private Vector3 returnPosition;

        public SitState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {
        }


        public override void Enter()
        {
            base.Enter();
            SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Sweep, 1.0f);
            controller.IsSitting = true;
            controller.GetComponent<CapsuleCollider>().isTrigger = true;
            if (controller.RecentlyDetectedProp != null)
                chair = controller.RecentlyDetectedProp;
            else
            {
                chair = GameManagerEx.Instance.StartChair;
            }
            
            chair.GetComponent<MeshCollider>().isTrigger = true;

            returnPosition = chair.transform.position + new Vector3(-chair.transform.localScale.x, controller.transform.position.y-chair.transform.position.y, 0);
            controller.transform.position = (chair.transform.position + Vector3.up * 0.55f);
            controller.SetPlayerRotation(chair.transform.rotation);
        }

        public override void Exit()
        {
            base.Exit();
            SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Sweep, 1.0f);
            controller.IsSitting = false;
            controller.SetPlayerPosition(returnPosition);
            controller.GetComponent<CapsuleCollider>().isTrigger = false;
            chair.GetComponent<MeshCollider>().isTrigger = false;
        }


        public override void HandleInput()
        {
            base.HandleInput();

            GetInteractOutInput(out isESCPressed);
            GetInteractableInput();                     // 앉아있는 상태에서 물건을 집을 수 있도록 추가함.
            GetMouseInput(out mouseX, out mouseY);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isESCPressed && !controller.IsGrabbing)
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