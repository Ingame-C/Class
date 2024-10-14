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
            if(Input.GetMouseButtonDown(0) && controller.IsDetectInteractable)
            {
                if (controller.RecentlyDetectedProp.GetComponent<Usable>() != null)
                {
                    controller.RecentlyDetectedProp.GetComponent<Usable>().Interact(controller);
                }

                // Grabbalbe Object는 일괄적으로 관리할 예정.
                if (controller.RecentlyDetectedProp.PropType >= PropTypes.Pencil)
                {
                    controller.GrabObject((Grabbable)controller.RecentlyDetectedProp);
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

            // Grabbalbe의 위치 조정.
            Vector3 _grabblePosition = controller.CameraTransform.position + controller.CameraTransform.forward * 0.5f;
            _grabblePosition += controller.CameraTransform.up * controller.GrabbaleHori + controller.CameraTransform.right * controller.GrabbaleVert;
            controller.InteractableGrabbing.transform.position = _grabblePosition;
        }

        #endregion
    }

}
