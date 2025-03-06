using UnityEngine;

namespace Class
{
    public class LockerDoor : Usable
    {
        #region Variables
        private int state = 0;      // Purpose: 여닫힌 상태를 더욱 편하게 다루는 용도.
        private Animator animator;
        private bool isAnimating = false;
        #endregion

        #region Properties
        public bool IsOpened => state == 1;
        public override PropTypes PropType => PropTypes.LockerDoor;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            animator = transform.parent.GetComponent<Animator>();
        }

        protected override void Init()
        {
        }
        #endregion

        #region Interaction
        public override void Interact(PlayerController controller)
        {
            if (isAnimating) return;

            state = (state + 1) % 2;
            isAnimating = true;

            if (state == 1)
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Locker_open, 1.0f);
                animator.SetBool(Constants.FLAG_LOCKERDOOR, true);
            }
            else
            {
                animator.SetBool(Constants.FLAG_LOCKERDOOR, false);
                Invoke(nameof(PlayCloseSound), 1.0f);
            }

            Invoke(nameof(ResetAnimationState), 1.0f);
        }
        #endregion

        #region Sound Effects
        private void PlayCloseSound()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Locker_close, 1.0f);
        }

        private void ResetAnimationState()
        {
            isAnimating = false;
        }
        #endregion
    }
}
