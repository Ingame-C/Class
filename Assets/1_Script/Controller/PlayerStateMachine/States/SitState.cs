using UnityEditor;
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
            controller.IsInteracting = true;
            SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Sweep);

            if (controller.RecentlyDetectedProp != null)
                chair = controller.RecentlyDetectedProp;
            else
            {
                chair = GameManagerEx.Instance.StartChair;
                Debug.Log(chair.name);
            }

            chair.GetComponent<BoxCollider>().isTrigger = true;

            returnPosition = chair.transform.position + new Vector3(-chair.transform.localScale.x, controller.transform.position.y-chair.transform.position.y, 0);
            controller.SetPlayerPosition(chair.transform.position);
            controller.SetPlayerRotation(chair.transform.rotation);
        }

        public override void Exit()
        {
            base.Exit();
            controller.IsInteracting = false;
            controller.SetPlayerPosition(returnPosition);
            chair.GetComponent<BoxCollider>().isTrigger = false;
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