using UnityEngine;

namespace Class.StateMachine {
    /// <summary>
    /// 플레이어가 디스맨과 상호작용하는 상태를 관리합니다.
    /// </summary>
    public class ThismanState : StateBase
    {
        #region Variables
        private Transform thismanTransform;
        #endregion

        #region Properties
        /// <summary>
        /// 현재 상호작용 중인 디스맨의 Transform입니다.
        /// </summary>
        public Transform ThismanTransform 
        { 
            get => thismanTransform; 
            set => thismanTransform = value; 
        }
        #endregion

        #region Constructor
        public ThismanState(PlayerController controller, PlayerStateMachine stateMachine, Animator animator)
            : base(controller, stateMachine, animator)
        {
        }
        #endregion

        // 게임 오버시, 각기 다른 디스맨 객체를 직접 지정해야 함. 
        // Transform 형식을 받으므로, 따로 부모 클래스를 만들 필요는 없을 것 같습니다.
        // GameManagerEx에서 변수를 입력합니다. 
        // 디스맨이 소환될 적엔, 플레이어의 카메라가 디스맨맨 쪽으로 고정 됨.
        #region State Methods
        public override void Enter()
        {
            base.Enter();
            InitializeThismanState();
        }

        public override void Exit()
        {
            base.Exit();
            CleanupThismanState();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            // 디스맨 상태에서는 입력을 받지 않습니다.
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // 디스맨 상태에서는 로직 업데이트가 필요하지 않습니다.
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // 디스맨 상태에서는 물리 업데이트가 필요하지 않습니다.
        }
        #endregion

        #region Private Methods
        private void InitializeThismanState()
        {
            // TODO: 카메라 회전을 부드럽게 처리하기 위해 Lerp 구현 필요
            controller.SetPlayerLookAt(thismanTransform);
        }

        private void CleanupThismanState()
        {
            // 필요한 정리 작업이 있다면 여기에 구현
        }
        #endregion
    }
}
