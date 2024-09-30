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

            // TODO : Lerp, set yaw
            controller.transform.LookAt(thisman);
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
