using System.Collections;
using System;
using System.Collections.Generic;
using Class.StateMachine;
using UnityEngine;

namespace Class
{
    [RequireComponent (typeof (Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {

        private Vector3 prevPosition;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerController playerController;

        [SerializeField] private float speed;
        private bool isWalking = false;


        void Start()
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
        void FixedUpdate()
        {
            Vector3 deltaPositon = transform.position - prevPosition;
            isWalking = (deltaPositon.magnitude / Time.deltaTime) > speed;

            // 걷기 애니메이션.
            if(playerController.StateMachine.CurState is WalkState walkState && isWalking)
            {
                animator.SetBool("IsWalking", true);
                if (playerController.StateMachine.CurState.VertInput < -0.05f)
                {
                    animator.SetBool("isWalkingBackward", true);
                    animator.SetBool("IsWalkingFrontRight", false);
                    animator.SetBool("IsWalkingFrontLeft", false);
                }
                else if (playerController.StateMachine.CurState.HorzInput < -0.05f)
                {
                    animator.SetBool("IsWalkingFrontLeft", true);
                }
                else if (playerController.StateMachine.CurState.HorzInput > 0.05f)
                {
                    animator.SetBool("IsWalkingFrontRight", true);
                }
                else
                {
                    animator.SetBool("isWalkingBackward", false);
                    animator.SetBool("IsWalkingFrontRight", false);
                    animator.SetBool("IsWalkingFrontLeft", false);
                }
            }
            else
            {
                animator.SetBool("IsWalkingFrontRight", false);
                animator.SetBool("IsWalkingFrontLeft", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("isWalkingBackward", false);
            }

            if (playerController.StateMachine.CurState is SitState)
            {
                animator.SetBool("IsSitting", true);
            }
            else
            {
                animator.SetBool("IsSitting", false);
            }


            prevPosition = transform.position;
        }
    }
}

