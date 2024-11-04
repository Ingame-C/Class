using UnityEngine;

namespace Class
{
    public abstract class Hidable : Usable
    {
        // Hidable한 object는 개수가 적을 것이라, Pos와 Rot를 이 안에서 다루겠습니다.
        [SerializeField] private Vector3 returnPosition;
        [SerializeField] private Vector3 hidePosition;
        [SerializeField] private Quaternion hideRotation;
        
        public Vector3 ReturnPosition { get => returnPosition; }
        public Vector3 HidePosition { get => hidePosition; }
        public Quaternion HideRotation { get => hideRotation; }
        public override void Interact(PlayerController controller)
        {
            controller.StateMachine.ChangeState(controller.hideState);
        }
    }
}