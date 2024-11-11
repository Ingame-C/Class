using UnityEngine;

namespace Class.StateMachine {
    public class FallState : StateBase
    {
        private Vector3 rotateAxis;
        private Vector3 rotatePoint;

        private float elapsedDegree;
        private float maxDegree = 150f;

        // TODO : 상호작용 중에는 어덯게 처리할까요?
        public FallState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {

        }

        public override void Enter()
        {
            base.Enter();
            controller.CapsuleColl.enabled = false;
            rotateAxis = controller.transform.TransformPoint(Vector3.right) - controller.transform.position;
            rotateAxis.y = 0;
            rotatePoint = controller.transform.position - new Vector3(0, controller.transform.localScale.y, 0);

            elapsedDegree = 0f;
        }

        public override void Exit()
        {
            base.Exit();
        }


        public override void HandleInput()
        {
            base.HandleInput();
            // No input while falling
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (elapsedDegree < maxDegree) elapsedDegree += controller.RotateAroundAxis(rotatePoint, -rotateAxis, elapsedDegree/maxDegree);
            else GameManagerEx.Instance.DirectSceneConversion(SceneEnums.Game);

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }

}
