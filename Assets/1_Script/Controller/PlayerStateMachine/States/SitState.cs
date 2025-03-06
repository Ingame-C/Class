using UnityEngine;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 앉기 상태를 관리합니다.
    /// </summary>
    public class SitState : StateBase
    {
        #region Variables
        private PropsBase chair;
        private Vector3 returnPosition;
        #endregion

        #region Constructor
        public SitState(PlayerController controller, PlayerStateMachine stateMachine)
            : base(controller, stateMachine)
        {
        }
        #endregion

        #region State Methods
        public override void Enter()
        {
            base.Enter();
            InitializeSitState();
        }

        public override void Exit()
        {
            base.Exit();
            CleanupSitState();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            GetInteractOutInput(out isESCPressed);
            GetInteractableInput();
            GetMouseInput(out mouseX, out mouseY);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            HandleRotationAndUI();
            HandleEscapeInput();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.RaycastInteractableObject();
        }
        #endregion

        #region Private Methods
        private void InitializeSitState()
        {
            PlaySitSound();
            SetupColliders();
            SetupChair();
            SetupPlayerPosition();
            AdjustCameraPosition();
        }

        private void CleanupSitState()
        {
            PlaySitSound();
            controller.IsSitting = false;
            controller.SetPlayerPosition(returnPosition);
            RestoreColliders();
            RestoreCameraPosition();
        }

        private void HandleRotationAndUI()
        {
            if (!controller.UIisSet)
            {
                controller.RotateWithMouse(mouseX, mouseY);
            }
            else if (isESCPressed)
            {
                controller.CurrentUI = null;
                isESCPressed = false;
                return;
            }
        }

        private void HandleEscapeInput()
        {
            if (isESCPressed && !controller.IsGrabbing)
            {
                stateMachine.ChangeState(controller.idleState);
                isESCPressed = false;
            }
        }

        private void PlaySitSound()
        {
            SoundManager.Instance.CreateAudioSource(controller.transform.position, SfxClipTypes.Sweep, 1.0f);
        }

        private void SetupColliders()
        {
            controller.IsSitting = true;
            controller.GetComponent<CapsuleCollider>().isTrigger = true;
        }

        private void SetupChair()
        {
            chair = controller.RecentlyDetectedProp ?? GameManagerEx.Instance.StartChair;
            chair.GetComponent<MeshCollider>().isTrigger = true;
        }

        private void SetupPlayerPosition()
        {
            returnPosition = chair.transform.position + new Vector3(-chair.transform.localScale.x, 
                controller.transform.position.y - chair.transform.position.y, 0);
            controller.transform.position = chair.transform.position + Vector3.up * 0.8f;
            controller.SetPlayerRotation(chair.transform.rotation);
        }

        private void AdjustCameraPosition()
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y - 0.3f,
                Camera.main.transform.position.z
            );
        }

        private void RestoreColliders()
        {
            controller.GetComponent<CapsuleCollider>().isTrigger = false;
            chair.GetComponent<MeshCollider>().isTrigger = false;
        }

        private void RestoreCameraPosition()
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y + 0.3f,
                Camera.main.transform.position.z
            );
        }
        #endregion
    }
}