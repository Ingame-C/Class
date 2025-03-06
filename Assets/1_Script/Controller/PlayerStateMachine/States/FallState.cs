using UnityEngine;

namespace Class.StateMachine {
    /// <summary>
    /// 플레이어의 추락 상태를 관리합니다.
    /// </summary>
    public class FallState : StateBase
    {
        #region Constants
        private const float MAX_FALL_DEGREE = 150f;
        #endregion

        #region Variables
        private Vector3 rotateAxis;
        private Vector3 rotatePoint;
        private float elapsedDegree;
        #endregion

        #region Constructor
        public FallState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {
        }
        #endregion

        #region State Methods
        public override void Enter()
        {
            base.Enter();
            InitializeFallState();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            // 추락 중에는 입력을 받지 않습니다.
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            HandleFallRotation();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        #endregion

        #region Private Methods
        private void InitializeFallState()
        {
            controller.CapsuleColl.enabled = false;
            rotateAxis = controller.transform.TransformPoint(Vector3.right) - controller.transform.position;
            rotateAxis.y = 0;
            rotatePoint = controller.transform.position - new Vector3(0, controller.transform.localScale.y, 0);
            elapsedDegree = 0f;
        }

        private void HandleFallRotation()
        {
            if (elapsedDegree < MAX_FALL_DEGREE)
            {
                elapsedDegree += controller.RotateAroundAxis(rotatePoint, -rotateAxis, elapsedDegree / MAX_FALL_DEGREE);
            }
            else
            {
                GameManagerEx.Instance.DirectSceneConversion(SceneEnums.Game);
            }
        }
        #endregion
    }
}
