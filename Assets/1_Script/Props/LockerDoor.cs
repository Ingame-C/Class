
using UnityEngine;

namespace Class
{
    public class LockerDoor : Usable
    {
        private int state = 0;      // Purpose: 여닫힌 상태를 더욱 편하게 다루는 용도.
        private Animator animator;
        public bool IsOpened {
            get
            {
                return state == 1;
            }
        }
        private PropTypes proptype = PropTypes.LockerDoor;
        public override PropTypes PropType { get => proptype; }


        private void Awake()
        {
            animator = transform.parent.GetComponent<Animator> ();
        }
        protected override void Init()
        {

        }
        public override void Interact(PlayerController controller)
        {
            state = (state + 1) % 2;
            animator.SetBool(Constants.FLAG_LOCKERDOOR, IsOpened);
            if (state == 1)
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Locker_open, 1.0f);
            }
            else
            {
                Invoke("LockerClose", 1.0f);
            }
        }

        private void LockerClose()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Locker_close, 1.0f);
        }
    }
}
