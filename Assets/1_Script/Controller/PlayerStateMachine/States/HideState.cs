using UnityEngine;

namespace Class.StateMachine
{
    public class HideState : StateBase
    {
        private Hidable hidable;

        public HideState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {}

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Hide Enter");
            controller.IsHiding = true;
            // TODO: 필요하다면 사운드 추가하기.
            //SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.???);
            hidable = (Hidable)controller.RecentlyDetectedProp;

            if(controller.IsGrabbing)
            {
                controller.InteractableGrabbing.ReleaseObject();
            }

            controller.SetPlayerPosition(hidable.HidePosition);
            controller.SetPlayerRotation(hidable.HideRotation);

            // 필요에 따라 없앨 수도 있을 것 같습니다.
            hidable.GetComponent<Collider>().isTrigger = true;
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Hide Exit");
            controller.IsHiding = false;
            controller.SetPlayerPosition(hidable.ReturnPosition);

            // 필요에 따라 없앨 수도 있을 것 같습니다.
            hidable.GetComponent<Collider>().isTrigger = false;
        }


        public override void HandleInput()
        {
            base.HandleInput();
            GetInteractOutInput(out isESCPressed);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isESCPressed)
            {
                stateMachine.ChangeState(controller.idleState);
                isESCPressed = false;
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

    }
}