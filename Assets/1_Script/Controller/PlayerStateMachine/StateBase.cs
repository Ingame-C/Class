using Unity.VisualScripting;
using UnityEngine;

namespace Class.StateMachine
{
    public class StateBase
    {
        protected PlayerController controller;      // Needed to control player (ex. move)
        protected PlayerStateMachine stateMachine;

        /** Input Values **/
        protected float vertInput = 0f;
        protected float horzInput = 0f;
        protected float vertInputRaw = 0f;
        protected float horzInputRaw = 0f;
        protected float mouseX = 0f;
        protected float mouseY = 0f;

        protected bool isESCPressed = false;

        public StateBase(PlayerController controller, PlayerStateMachine stateMachine)
        {
            this.controller = controller;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }             // Run once when Enter State
        public virtual void HandleInput() { }       // Manage Input in particular state
        public virtual void LogicUpdate()           // Logic Update
        {         
            HoldGrabbable();

            // Unexpect Error: IsGrabbing이 false라면 의자에서 내리는 게 불가능 해야 하는데, false가 되면서 내리는 것까지 되는 오류가 발생함.
            // Solution : 물건을 집는 것에 쿨타임 삽입. 그 쿨타임 내로 천천히 IsGrabbing을 false로 만들면 해결 될듯함.
            if (Input.GetMouseButtonDown(1) && controller.IsGrabbing)
            {
                controller.ReleaseObject();
            }
        }       
        public virtual void PhysicsUpdate() { }     // Only Physics Update
        public virtual void Exit() { }              // Run once when Exit State


        #region Input Modules

        protected void GetMouseInput(out float mouseX, out float mouseY)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        protected void GetMovementInputRaw(out float vert, out float horz)
        {
            vert = Input.GetAxisRaw("Vertical");
            horz = Input.GetAxisRaw("Horizontal");
        }

        protected void GetMovementInput(out float vert, out float horz)
        {
            vert = Input.GetAxis("Vertical");
            horz = Input.GetAxis("Horizontal");
        }

        protected void GetInteractableInput()
        {
            // Message: 앉아있는 상태에서 물건을 집는 것이 불가능해지기에 우선 주석처리를 함. 나중에 회의를 해서 코드 수정 요망.
            // if (controller.IsInteracting) return; 

            if(Input.GetMouseButtonDown(0) && controller.IsDetectInteractable)
            {
                Debug.Log("clicked!");
                switch (controller.RecentlyDetectedProp.PropType) {
                    case PropTypes.Chair:
                        stateMachine.ChangeState(controller.sitState);
                        break;
                    case PropTypes.Pencil:
                        controller.GrabObject((Grabbable)controller.RecentlyDetectedProp);
                        break;
                }

            }
        }

        protected void GetInteractOutInput(out bool isPressed)
        {
            isPressed = false;
            if (Input.GetMouseButtonDown(1))
            {
                isPressed = true;
            }
        }

        private void HoldGrabbable()
        {
            if (controller.InteractableGrabbing == null)
            {
                return;
            }

            // To do: 들고있는 상태 동안의 로직 구현하기
            controller.InteractableGrabbing.transform.position = controller.CameraTransform.position + controller.CameraTransform.forward * 0.5f;
        }

        #endregion
    }

}
