
using UnityEngine;

namespace Class
{
    [RequireComponent (typeof (Animator))]
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
            animator.SetBool("IsOpened", IsOpened);
        }
    }
}
