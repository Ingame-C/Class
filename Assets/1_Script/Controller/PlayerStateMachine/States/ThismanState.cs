using UnityEngine;

namespace Class.StateMachine {
    public class ThismanState : StateBase
    {

        public ThismanState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {

        }


        // 게임 오버시, 각기 다른 디스맨 객체를 직접 지정해야 함. 
        // Transform 형식을 받으므로, 따로 부모 클래스를 만들 필요는 없을 것 같습니다.
        // GameManagerEx에서 변수를 입력합니다.
        private Transform thisman;
        public Transform Thisman { get => thisman; set => thisman = value; }    

        // 디스맨이 소환될 적엔, 플레이어의 카메라가 디스멘 쪽으로 고정 됨.
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
