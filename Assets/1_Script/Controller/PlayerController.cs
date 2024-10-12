using Class.StateMachine;
using System;
using UnityEngine;

namespace Class
{

    [RequireComponent (typeof(Rigidbody))]
    [RequireComponent (typeof(CapsuleCollider))]
    [RequireComponent (typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {

        /** Components **/
        private Rigidbody rigid;
        private CapsuleCollider capsuleColl;
        private Animator animator;

        /** Properties **/
        public Rigidbody Rigidbody { get => rigid; }
        public CapsuleCollider CapsuleColl {  get { return capsuleColl; } }
        public Animator Animator { get => animator; }




        [Header("Player Move Args")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float horzRotSpeed;
        [SerializeField] private float vertRotSpeed;
        [SerializeField, Range(0f, 90f)] private float maxVertRot;
        [SerializeField, Range(-90f, 0f)] private float minVertRot;

        [Header("GameObjects")]
        [SerializeField] private Transform cameraTransform;
        
        [Header("Raycast Args")]                        // Use to detect Interactables
        [SerializeField] private float rayLength;


        private PlayerStateMachine stateMachine;
        public PlayerStateMachine StateMachine { get => stateMachine; }
        public IdleState idleState;
        public WalkState walkState;
        public SitState sitState;

        public FallState fallState;
        public ThismanState thismanState;



        private bool isDetectInteractable = false;
        private bool isInteracting = false;

        public bool IsDetectInteractable { get => isDetectInteractable; }
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }

        private void Awake()
        {
            recentlyDetectedProp = null;

            rigid = GetComponent<Rigidbody>();
            capsuleColl = GetComponent<CapsuleCollider>();
            
            stateMachine = new PlayerStateMachine();
            idleState = new IdleState(this, stateMachine);
            walkState = new WalkState(this, stateMachine);
            sitState = new SitState(this, stateMachine);
            thismanState = new ThismanState(this, stateMachine);
            fallState = new FallState(this, StateMachine);

        }

        private void Start()
        {
            stateMachine.Init(sitState);
            GameManagerEx.Instance.OnStageFailAction += ChangeStateToThisman;
            GameManagerEx.Instance.OnStageClearAction += ChangeStateToFall;
        }

        private void Update()
        {
            stateMachine.CurState.HandleInput();

            stateMachine.CurState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            stateMachine.CurState.PhysicsUpdate();
        }


        #region Set Funcs

        public void SetPlayerPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetPlayerRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetPlayerLookAt(Transform targetTransform)
        {
            transform.LookAt(targetTransform);
            cameraTransform.LookAt(targetTransform);
        }

        private void ChangeStateToThisman()
        {
            stateMachine.ChangeState(thismanState);
        }

        private void ChangeStateToFall()
        {
            stateMachine.ChangeState(fallState);
        }

        #endregion


        #region Logic Control Funcs

        private float horzRot = 0f;
        private float vertRot = 0f;
        public void RotateWithMouse(float mouseX, float mouseY)
        {
            horzRot += mouseX * horzRotSpeed;
            vertRot -= mouseY * vertRotSpeed;


            vertRot = Mathf.Clamp(vertRot, minVertRot, maxVertRot);

            transform.rotation = Quaternion.Euler(0f, horzRot, 0f);
            cameraTransform.rotation = Quaternion.Euler(vertRot, horzRot, 0f);
        }

        private float fallSpeed = 50f;
        public float RotateAroundAxis(Vector3 point, Vector3 dir, float ratio)
        {
            float tmpTime = Time.deltaTime * (fallSpeed + fallSpeed * 1.75f* Mathf.Sin(ratio*Mathf.PI));
            transform.RotateAround(point, dir, tmpTime);

            return tmpTime;
        }

        #endregion


        #region Physics Control Funcs

        public void WalkWithArrow(float vertInputRaw, float horzInputRaw, float diag)
        {
            Vector3 moveDir = (transform.forward * horzInputRaw + transform.right * vertInputRaw);

            rigid.MovePosition(transform.position + moveDir * diag * walkSpeed * Time.fixedDeltaTime);

        }

        private PropsBase recentlyDetectedProp = null;
        public PropsBase RecentlyDetectedProp { get => recentlyDetectedProp; }

        public void RaycastInteractableObject()
        {
            RaycastHit hit;
            int targetLayer = Constants.LAYER_INTERACTABLE;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, targetLayer))
            {
                isDetectInteractable = true;
                recentlyDetectedProp = hit.transform.GetComponent<PropsBase>();
            }
            else
            {
                isDetectInteractable = false;
                recentlyDetectedProp = null;
            }

            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * rayLength, Color.red);
        }

        #endregion


        #region Grabbing Fields and Funcs

        public Grabbable InteractableGrabbing = null;
        public bool IsGrabbing = false;
        public Transform CameraTransform { get => cameraTransform; }        // 들고있는 물체의 위치를 정돈시키기 위해, 카메라의 트랜스폼을 가져옴.

        public void GrabObject(Grabbable grabbable)
        {
            if (IsGrabbing || InteractableGrabbing != null)
            {
                return;
            }

            IsGrabbing = true;
            InteractableGrabbing = grabbable;
            InteractableGrabbing.GetComponent<BoxCollider>().isTrigger = true;
        }

        public void ReleaseObject()
        {
            if(InteractableGrabbing == null || !IsGrabbing)
            {
                return;
            }

            InteractableGrabbing.GetComponent<BoxCollider>().isTrigger = false;
            InteractableGrabbing = null;

            Invoke("DelaySetFlag", 0.5f);   // IsGrabbing 설정을 딜레이 시켜서, 의자를 내리지 못하게 설정.
        }

        public void DelaySetFlag()
        {
            IsGrabbing = false;
        }


        #endregion
    }
}