using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 모든 상태의 기본 클래스입니다.
    /// 입력 처리와 기본 동작을 정의합니다.
    /// </summary>
    public class StateBase
    {
        #region Event
        
        public event UnityAction OnExit  = delegate{};
        public event UnityAction OnEnter = delegate{};
        
        #endregion
        
        #region Constants
        protected const float GRABBLE_DISTANCE = 0.2f;
        #endregion

        #region Protected Fields
        protected PlayerController controller;
        protected PlayerStateMachine stateMachine;
        protected Animator animator;
        #endregion

        #region Input Values
        protected float vertInput;
        protected float horzInput;
        protected float vertInputRaw;
        protected float horzInputRaw;
        protected float mouseX;
        protected float mouseY;
        protected bool isESCPressed;

        public float VertInput => vertInput;
        public float HorzInput => horzInput;
        public float VertInputRaw => vertInputRaw;
        public float HorzInputRaw => horzInputRaw;
        public float MouseX => mouseX;
        public float MouseY => mouseY;
        #endregion

        #region Constructor
        public StateBase(PlayerController controller, PlayerStateMachine stateMachine, Animator animator)
        {
            this.controller = controller;
            this.stateMachine = stateMachine;
            this.animator = animator;
        }
        #endregion

        #region Virtual Methods

        public virtual void Enter()
        {
            OnEnter?.Invoke();
        }
        public virtual void HandleInput() { }
        public virtual void PhysicsUpdate() { }

        public virtual void Exit()
        {
            OnExit?.Invoke();
        }

        public virtual void LogicUpdate()
        {         
            HandleGrabbableObject();
            HandleInteractionInput();
            UpdateUI();
        }
        #endregion

        #region Input Handling
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
            if (Input.GetMouseButtonDown(0) && controller.IsDetectInteractable)
            {
                HandleInteractableObject();
            }
        }

        protected void GetInteractOutInput(out bool isPressed)
        {
            isPressed = Input.GetMouseButtonDown(1);
        }
        #endregion

        #region Logic Update Methods
        private void HandleGrabbableObject()
        {
            if (controller.InteractableGrabbing == null) return;

            UpdateGrabbablePosition();
            HandleGrabbableRelease();
        }

        private void UpdateGrabbablePosition()
        {
            Vector3 grabblePosition = controller.CameraTransform.position + 
                                    controller.CameraTransform.forward * GRABBLE_DISTANCE +
                                    controller.CameraTransform.up * controller.GrabbaleHori + 
                                    controller.CameraTransform.right * controller.GrabbaleVert;
            
            controller.InteractableGrabbing.transform.position = grabblePosition;
        }

        private void HandleGrabbableRelease()
        {
            if (Input.GetMouseButtonDown(1) && 
                controller.InteractableGrabbing is Grabbable grabbable && 
                !controller.UIisSet)
            {
                grabbable.ReleaseObject();
            }
        }

        private void HandleInteractionInput()
        {
            if (Input.GetMouseButtonDown(0) && controller.IsDetectInteractable)
            {
                HandleInteractableObject();
            }
        }

        private void HandleInteractableObject()
        {
            if (controller.RecentlyDetectedProp is Usable usable)
            {
                usable.Interact(controller);
            }

            if (controller.RecentlyDetectedProp.TryGetComponent<Grabbable>(out Grabbable grabbable))
            {
                grabbable.GrabObject();
            }
        }

        private void UpdateUI()
        {
            if (controller.UIisSet)
            {
                controller.CurrentUI.LogicUpdate();
            }
        }
        #endregion
    }
}