using System;
using Class.StateMachine;
using UnityEngine;

namespace Class
{
    /// <summary>
    /// 플레이어의 애니메이션을 제어하는 컨트롤러입니다.
    /// 걷기, 앉기 등의 애니메이션 상태를 관리합니다.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Animation Parameters
        private static readonly string IS_WALKING = "IsWalking";
        private static readonly string IS_SITTING = "IsSitting";

        private int idle;
        private int walking;
        
        #endregion

        #region Components
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerController playerController;
        #endregion

        #region Movement Parameters
        [SerializeField] private float walkSpeedThreshold;
        private float hori;
        private float vert;
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeComponents();
        }

        private void FixedUpdate()
        {
            UpdateAnimationState();
        }
        #endregion

        #region Initialization
        private void InitializeComponents()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            
            if (!playerController)
            {
                playerController = transform.parent.GetComponent<PlayerController>();
            }

            idle = Animator.StringToHash("Idle");
            walking = Animator.StringToHash("Walking");

        }
        #endregion
        

        #region Animation State
        private void UpdateAnimationState()
        {
            UpdateWalkAnimation();
            UpdateSitAnimation();
        }

        private void UpdateWalkAnimation()
        {
            UpdateWalkDirectionAnimation();
            animator.SetBool(IS_WALKING, (playerController.StateMachine.CurrentState is WalkState));
        }

        private void UpdateWalkDirectionAnimation()
        {
            vert = Input.GetAxis("Vertical");
            hori = Input.GetAxis("Horizontal");
            
            var movement = new Vector2(hori, vert);
            movement.Normalize();
            
            animator.SetFloat("XDir", movement.x);
            animator.SetFloat("YDir", movement.y);

            if (movement.magnitude == 0f && animator.GetBool(IS_WALKING))
            {
                animator.CrossFade(idle, 0.7f);
                Debug.Log("스톱!");
                return;
            }
            
            if(!Mathf.Approximately(movement.magnitude, 0f) && !animator.GetBool(IS_WALKING))
            {
                animator.CrossFade(walking, 0.7f);
                return;
            }
        }
        
        private void UpdateSitAnimation()
        {
            bool isSitting = playerController.StateMachine.CurrentState is SitState;
            animator.SetBool(IS_SITTING, isSitting);
        }
        #endregion
    }
}

