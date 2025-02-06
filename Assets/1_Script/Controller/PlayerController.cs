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

        /** Components **/
        private Rigidbody rigid;
        private CapsuleCollider capsuleColl;
        private Animator animator;
        

        /** Properties **/
        public Rigidbody Rigidbody { get => rigid; }
        public CapsuleCollider CapsuleColl {  get { return capsuleColl; } }
        public Animator Animator { get => animator; }
        
        
        public UI.UI CurrentUI
        {
            get => currentUI;
            set
            {
                if (value == null)
                {
                    currentUI.gameObject.SetActive(false);
                    
                }
                else
                {
                    value.gameObject.SetActive(true);
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
        

        public Grabbable InteractableGrabbing { get => interactableGrabbing; set { interactableGrabbing = value; } }
        public bool IsGrabbing { get => isGrabbing; set { isGrabbing = value; } }
        public bool IsSitting { get => isSitting; set => isSitting = value; }
        public bool IsHiding { get => isHiding; set => isHiding = value; }
        public bool IsDetectInteractable { get => isDetectInteractable; }
        
        public Transform CameraTransform { get => cameraTransform; }



        private void Awake()
        {
            recentlyDetectedProp = null;
            currentUI = null;

            rigid = GetComponent<Rigidbody>();
            capsuleColl = GetComponent<CapsuleCollider>();
            
            stateMachine = new PlayerStateMachine();
            idleState = new IdleState(this, stateMachine);
            walkState = new WalkState(this, stateMachine);
            sitState = new SitState(this, stateMachine);
            thismanState = new ThismanState(this, stateMachine);
            fallState = new FallState(this, StateMachine);
            hideState = new HideState(this, stateMachine);
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
                rigid.MovePosition(transform.position + moveDir * diag * walkSpeed * 0.5f * Time.fixedDeltaTime);
            }
            

        }

        private PropsBase recentlyDetectedProp = null;
        public PropsBase RecentlyDetectedProp { get => recentlyDetectedProp; }

        private PropsBase prevDetectedProp = null;


        public void RaycastInteractableObject()
        {
            RaycastHit hit;
            int targetLayer = Constants.LAYER_INTERACTABLE + Constants.LAYER_STAGE1SCHOOLSUPPLIES + Constants.LAYER_COLLISIONIMPOSSIBLE;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLength, targetLayer))
            {
                isDetectInteractable = true;
                recentlyDetectedProp = hit.transform.GetComponent<PropsBase>();

                if (prevDetectedProp != null && recentlyDetectedProp != prevDetectedProp) 
                {
                    if (prevDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline1))
                    {
                        outline1.OutlineMode = Outline.Mode.OutlineHidden;
                        outline1.OutlineWidth = 0f;
                    }
                }

                if (recentlyDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline))
                {
                    outline.OutlineMode = Outline.Mode.OutlineAll;
                    outline.OutlineColor = Color.red;
                    outline.OutlineWidth = 5f;
                }

                prevDetectedProp = recentlyDetectedProp;
            }
            else
            {                
                if (recentlyDetectedProp != null && recentlyDetectedProp.gameObject.TryGetComponent<Outline>(out Outline outline)) 
                {
                    outline.OutlineMode = Outline.Mode.OutlineHidden;
                    outline.OutlineWidth = 0f;
                }

                isDetectInteractable = false;
                recentlyDetectedProp = null;
            }

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