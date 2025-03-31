using System.Threading;
using UnityEngine;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 이동 상태를 관리합니다.
    /// </summary>
    public class WalkState : StateBase
    {
        #region Constants
        private const float DIAGONAL_MOVEMENT_FACTOR = 0.71f;
        private const float MOVEMENT_THRESHOLD = 0.5f;
        private const float BACKWARD_SOUND_MULTIPLIER = 1.2f;
        private const float DEFAULT_SOUND_INTERVAL = 0.472f;

        private int walkHash;
        #endregion

        #region Variables
        private float diagonalWeight;
        private float currentSpeed;
        private float lastSoundTime = -Mathf.Infinity;
        private float soundInterval = DEFAULT_SOUND_INTERVAL;
        private int randomWalkSoundIndex = 1;
        private float hori;
        private float vert;
        #endregion

        #region Constructor
        public WalkState(PlayerController controller, PlayerStateMachine stateMachine, Animator animator)
            : base(controller, stateMachine, animator)
        {
            walkHash = Animator.StringToHash("Walk");
        }
        #endregion

        #region State Methods
        public override void Enter()
        {
            base.Enter();
            InitializeWalkState();
            animator.CrossFade(walkHash, 0.01f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            GetInteractOutInput(out isESCPressed);
            GetMovementInput(out vertInput, out horzInput);
            GetMovementInputRaw(out vertInputRaw, out horzInputRaw);
            GetMouseInput(out mouseX, out mouseY);
            GetInteractableInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            HandleMovementState();
            HandleUIState();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            HandleFootstepSound();
            controller.RaycastInteractableObject();
            UpdateDiagonalMovement();
            HandleMovement();
        }
        #endregion

        #region Private Methods
        private void InitializeWalkState()
        {
            ResetMovementInput();
            diagonalWeight = 1.0f;
        }

        private void HandleMovementState()
        {
            currentSpeed = Mathf.Abs(vertInput) + Mathf.Abs(horzInput);
            
            vert = Input.GetAxis("Vertical");
            hori = Input.GetAxis("Horizontal");
            
            var movement = new Vector2(hori, vert);
            movement.Normalize();
            
            animator.SetFloat("XDir", movement.x);
            animator.SetFloat("YDir", movement.y);
            
            if (Mathf.Approximately(currentSpeed, 0f))
            {
                stateMachine.ChangeState(controller.idleState);
            }
        }

        private void HandleMovement()
        {
            if (!controller.UIisSet)
            {
                controller.WalkWithArrow(horzInputRaw, vertInputRaw, diagonalWeight);
            }
        }

        private void HandleUIState()
        {
            if (isESCPressed && controller.UIisSet)
            {
                controller.CurrentUI = null;
                isESCPressed = false;
            }
            
            if (!controller.UIisSet)
            {
                controller.RotateWithMouse(mouseX, mouseY);
            }
        }

        private void HandleFootstepSound()
        {
            if (Time.time - lastSoundTime >= soundInterval && !controller.UIisSet)
            {
                PlayRandomFootstepSound();
                UpdateSoundInterval();
                lastSoundTime = Time.time;
            }
        }

        private void PlayRandomFootstepSound()
        {
            randomWalkSoundIndex = Random.Range(1, 4);
            switch (randomWalkSoundIndex)
            {
                case 1:
                    SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Player_Walk_1, 1.0f);
                    soundInterval = 0.472f;
                    break;
                case 2:
                    SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Player_Walk_2, 1.0f);
                    soundInterval = 0.503f;
                    break;
                case 3:
                    SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Player_Walk_3, 1.0f);
                    soundInterval = 0.382f;
                    break;
            }
        }

        private void UpdateSoundInterval()
        {
            if (vertInput < 0)
            {
                soundInterval *= BACKWARD_SOUND_MULTIPLIER;
            }
        }

        private void UpdateDiagonalMovement()
        {
            diagonalWeight = (Mathf.Abs(horzInput) > MOVEMENT_THRESHOLD && 
                            Mathf.Abs(vertInput) > MOVEMENT_THRESHOLD) ? 
                            DIAGONAL_MOVEMENT_FACTOR : 1.0f;
        }

        private void ResetMovementInput()
        {
            vertInput = horzInput = vertInputRaw = horzInputRaw = 0f;
        }
        #endregion
    }
}
