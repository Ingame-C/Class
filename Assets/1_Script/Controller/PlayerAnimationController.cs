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
        private static readonly string IS_WALKING_BACKWARD = "isWalkingBackward";
        private static readonly string IS_WALKING_FRONT_RIGHT = "IsWalkingFrontRight";
        private static readonly string IS_WALKING_FRONT_LEFT = "IsWalkingFrontLeft";
        private static readonly string IS_SITTING = "IsSitting";
        #endregion

        #region Components
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerController playerController;
        #endregion

        #region Movement Parameters
        [SerializeField] private float walkSpeedThreshold;
        private Vector3 prevPosition;
        private bool isWalking;
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeComponents();
        }

        private void FixedUpdate()
        {
            UpdateMovementState();
            UpdateAnimationState();
            prevPosition = transform.position;
        }
        #endregion

        #region Initialization
        private void InitializeComponents()
        {
            prevPosition = transform.position;
            
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            
            if (!playerController)
            {
                playerController = transform.parent.GetComponent<PlayerController>();
            }
        }
        #endregion

        #region Movement State
        private void UpdateMovementState()
        {
            Vector3 deltaPosition = transform.position - prevPosition;
            isWalking = (deltaPosition.magnitude / Time.deltaTime) > walkSpeedThreshold;
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
            if (playerController.StateMachine.CurrentState is WalkState && isWalking)
            {
                animator.SetBool(IS_WALKING, true);
                UpdateWalkDirectionAnimation();
            }
            else
            {
                ResetWalkAnimation();
            }
        }

        private void UpdateWalkDirectionAnimation()
        {
            var currentState = playerController.StateMachine.CurrentState;
            
            if (currentState.VertInput < -0.05f)
            {
                animator.SetBool(IS_WALKING_BACKWARD, true);
                animator.SetBool(IS_WALKING_FRONT_RIGHT, false);
                animator.SetBool(IS_WALKING_FRONT_LEFT, false);
            }
            else if (currentState.HorzInput < -0.05f)
            {
                animator.SetBool(IS_WALKING_FRONT_LEFT, true);
            }
            else if (currentState.HorzInput > 0.05f)
            {
                animator.SetBool(IS_WALKING_FRONT_RIGHT, true);
            }
            else
            {
                ResetWalkDirectionAnimation();
            }
        }

        private void ResetWalkAnimation()
        {
            animator.SetBool(IS_WALKING_FRONT_RIGHT, false);
            animator.SetBool(IS_WALKING_FRONT_LEFT, false);
            animator.SetBool(IS_WALKING, false);
            animator.SetBool(IS_WALKING_BACKWARD, false);
        }

        private void ResetWalkDirectionAnimation()
        {
            animator.SetBool(IS_WALKING_BACKWARD, false);
            animator.SetBool(IS_WALKING_FRONT_RIGHT, false);
            animator.SetBool(IS_WALKING_FRONT_LEFT, false);
        }

        private void UpdateSitAnimation()
        {
            bool isSitting = playerController.StateMachine.CurrentState is SitState;
            animator.SetBool(IS_SITTING, isSitting);
        }
        #endregion
    }
}

