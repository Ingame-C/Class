using Class.StateMachine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Class
{

    [RequireComponent (typeof(Rigidbody))]
    [RequireComponent (typeof(CapsuleCollider))]
    [RequireComponent (typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {

        #region Components
        private Rigidbody rigid;
        private CapsuleCollider capsuleColl;
        #endregion

        /** Properties **/
        public Rigidbody Rigidbody { get => rigid; }
        public CapsuleCollider CapsuleColl { get => capsuleColl; }
        
        
        public float MaxVertRot
        {
            get => maxVertRot;
            set => maxVertRot = value;
        }

        public Vector3 RaycastHitPosition { get => raycastHitPosition; }
        
        
        public UI.UI CurrentUI
        {
            get => currentUI;
            set
            {
                if (value == null)
                {
                    currentUI.gameObject.SetActive(false);
                    Cursor.visible = false;
                }
                else
                {
                    value.gameObject.SetActive(true);
                    Cursor.visible = true;
                }
                currentUI = value;
            } 
        }
        private UI.UI currentUI;

        [Header("Player Move Args")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float horzRotSpeed;
        [SerializeField] private float vertRotSpeed;
        [SerializeField, Range(0f, 90f)] private float maxVertRot;
        [SerializeField, Range(-90f, 0f)] private float minVertRot;
        [SerializeField] public float GrabbaleHori = 0f;
        [SerializeField] public float GrabbaleVert = 0f;

        [Header("GameObjects")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private GameObject bloodyRain;
        
        [Header("Raycast Args")]                        // Use to detect Interactables
        [SerializeField] private float rayLength;
        
        [Header("Animator")]      
        [SerializeField] public Animator Animator;

        private PlayerStateMachine stateMachine;
        public PlayerStateMachine StateMachine { get => stateMachine; }
        public IdleState idleState;
        public WalkState walkState;
        public SitState sitState;
        public HideState hideState;

        public FallState fallState;
        public ThismanState thismanState;

        private Grabbable interactableGrabbing = null;

        public bool UIisSet { get => CurrentUI != null; }
        private bool isDetectInteractable = false;
        private bool isSitting = false;
        private bool isGrabbing = false;
        private bool isHiding = false;
        
        private Vector3 raycastHitPosition;
        

        public Grabbable InteractableGrabbing { get => interactableGrabbing; set { interactableGrabbing = value; } }
        public bool IsGrabbing { get => isGrabbing; set { isGrabbing = value; } }
        public bool IsSitting { get => isSitting; set => isSitting = value; }
        public bool IsHiding { get => isHiding; set => isHiding = value; }
        public bool IsDetectInteractable { get => isDetectInteractable; }
        
        public Transform CameraTransform { get => cameraTransform; }


        #region Unity Methods
        private void Awake()
        {
            InitializeVariables();
            InitializeComponents();
            InitializeStateMachine();
        }
        private void Start()
        {
            InitializeStateMachineState();
            RegisterGameManagerEvents();
        }
        private void Update()
        {
            stateMachine.CurrentState.HandleInput();
            stateMachine.CurrentState.LogicUpdate();
        }
        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate();
        }
        #endregion

        #region Initialize
        private void InitializeVariables()
        {
            recentlyDetectedProp = null;
            currentUI = null;
        }

        private void InitializeComponents()
        {
            rigid = GetComponent<Rigidbody>();
            capsuleColl = GetComponent<CapsuleCollider>();
        }

        private void InitializeStateMachine()
        {
            stateMachine = new PlayerStateMachine();
            idleState = new IdleState(this, stateMachine, Animator);
            walkState = new WalkState(this, stateMachine, Animator);
            sitState = new SitState(this, stateMachine, Animator);
            thismanState = new ThismanState(this, stateMachine, Animator);
            fallState = new FallState(this, StateMachine, Animator);
            hideState = new HideState(this, stateMachine, Animator);
        }
        private void InitializeStateMachineState()
        {
            stateMachine.Init(sitState);
        }

        private void RegisterGameManagerEvents()
        {
            GameManagerEx.Instance.OnStageFailAction += ChangeStateToThisman;
            GameManagerEx.Instance.OnStageClearAction += ChangeStateToFall;
        }
        #endregion

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

        public void SetBloodyRain(bool isActive)
        {
            bloodyRain.SetActive(isActive);
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

            if(horzInputRaw > 0)
            {
                rigid.MovePosition(transform.position + moveDir * diag * walkSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rigid.MovePosition(transform.position + moveDir * diag * walkSpeed * 0.8f * Time.fixedDeltaTime);
            }
        }

        private PropsBase recentlyDetectedProp = null;
        public PropsBase RecentlyDetectedProp { get => recentlyDetectedProp; }
        private PropsBase prevDetectedProp = null;

        /// <summary>
        /// 상호작용 가능한 오브젝트를 레이캐스트로 감지하고 아웃라인을 처리합니다.
        /// </summary>
        public void RaycastInteractableObject()
        {
            PerformRaycast();
            DrawDebugRay();
        }

        /// <summary>
        /// 레이캐스트를 수행하고 결과를 처리합니다.
        /// </summary>
        private void PerformRaycast()
        {
            RaycastHit hit;
            int targetLayer = Constants.LAYER_INTERACTABLE + Constants.LAYER_STAGE1SCHOOLSUPPLIES + Constants.LAYER_COLLISIONIMPOSSIBLE;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, targetLayer))
            {
                HandleRaycastHit(hit);
            }
            else
            {
                HandleRaycastMiss();
            }
        }

        /// <summary>
        /// 레이캐스트 히트 시 처리를 담당합니다.
        /// </summary>
        private void HandleRaycastHit(RaycastHit hit)
        {
            isDetectInteractable = true;
            recentlyDetectedProp = hit.transform.GetComponent<PropsBase>();

            UpdatePreviousPropOutline();
            UpdateCurrentPropOutline();
            
            prevDetectedProp = recentlyDetectedProp;
            
            raycastHitPosition = hit.transform.position;
        }

        /// <summary>
        /// 이전 프롭의 아웃라인을 업데이트합니다.
        /// </summary>
        private void UpdatePreviousPropOutline()
        {
            if (prevDetectedProp != null && recentlyDetectedProp != prevDetectedProp)
            {
                if (prevDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline))
                {
                    HideOutline(outline);
                }
            }
        }

        /// <summary>
        /// 현재 프롭의 아웃라인을 업데이트합니다.
        /// </summary>
        private void UpdateCurrentPropOutline()
        {
            if (recentlyDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline))
            {
                ShowOutline(outline);
            }
        }

        /// <summary>
        /// 레이캐스트 미스 시 처리를 담당합니다.
        /// </summary>
        private void HandleRaycastMiss()
        {
            if (recentlyDetectedProp != null && recentlyDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline))
            {
                HideOutline(outline);
            }

            isDetectInteractable = false;
            recentlyDetectedProp = null;
        }

        /// <summary>
        /// 아웃라인을 표시합니다.
        /// </summary>
        private void ShowOutline(Outline outline)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
        }

        /// <summary>
        /// 아웃라인을 숨깁니다.
        /// </summary>
        private void HideOutline(Outline outline)
        {
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            outline.OutlineWidth = 0f;
        }

        /// <summary>
        /// 디버그 레이를 그립니다.
        /// </summary>
        private void DrawDebugRay()
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * rayLength, Color.red);
        }

        // 닿아선 안 될 오브젝트에 닿았을 때, 게임 오버 시키는 용도입니다.
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag(Constants.TAG_LAVAOBJECT))
            {
                // TODO: GAMEOVER 시켜야 합니다.
            }
        }
        #endregion
    }
}