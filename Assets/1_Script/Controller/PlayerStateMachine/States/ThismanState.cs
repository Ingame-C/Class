using UnityEngine;

namespace Class.StateMachine {
    public class ThismanState : StateBase
    {

        public ThismanState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {

        }


        private Transform thisman;
        public Transform Thisman { get => thisman; set => thisman = value; }    

        public override void Enter()
        {
            base.Enter();

            // TODO : 바로 Lookat이 아니라 LookAt함수로 Quaternion 받아와서 Update에서 Lerp해도 좋을 것 같습니다.
            controller.SetPlayerLookAt(thisman);
        }

        public override void Exit()
        {
            base.Exit();
        }


        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
