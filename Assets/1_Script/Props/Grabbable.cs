using UnityEngine;

namespace Class
{
    [RequireComponent (typeof (Rigidbody))]
    public abstract class Grabbable : PropsBase
    {
        private Rigidbody _rigidbody;
        public Desk TheDeskBelow;
        public bool isOnTheDesk { get => (TheDeskBelow == null); }
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody> ();
        }
        private void OnCollisionEnter(Collision collision)
        {
            _rigidbody.velocity = Vector3.zero;
        }

    }
}
