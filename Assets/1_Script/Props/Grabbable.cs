using UnityEngine;

namespace Class
{
    [RequireComponent (typeof (Rigidbody))]
    public abstract class Grabbable : PropsBase
    {
        private Rigidbody rigid = null;
        private Desk desk = null;
        private PlayerController controller = null;

        public Desk TheDeskBelow { get => desk; }
        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            controller = GameObject.Find(Constants.NAME_PLAYER).GetComponent<PlayerController>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            rigid.velocity = Vector3.zero;
        }

        public void GrabObject()
        {
            if (controller.IsGrabbing)
            {
                return;
            }

            // 만약 물건이 책상 아래에 놓여져 있었다면, 책상 위 목록에서 grabbable을 삭제.
            if (desk != null)
            {
                TheDeskBelow.props.Remove(this.PropType);
            }

            controller.InteractableGrabbing = this;
            controller.InteractableGrabbing.GetComponent<BoxCollider>().isTrigger = true;
            controller.IsGrabbing = true;
        }

        public void ReleaseObject()
        {
            float distance = 1.0f;

            Vector3 releasePosion = controller.CameraTransform.position;

            if (controller.InteractableGrabbing == null || !controller.IsGrabbing)
            {
                return;
            }

            if (controller.RecentlyDetectedProp is Desk desk)
            {
                distance = Vector3.Distance(controller.transform.position, desk.transform.position);
                this.desk = desk;
                this.desk.props.Add(this.PropType);
            }

            releasePosion += controller.CameraTransform.forward * distance + Vector3.up * 0.1f;

            controller.InteractableGrabbing.transform.position = releasePosion;
            controller.InteractableGrabbing.GetComponent<BoxCollider>().isTrigger = false;
            controller.InteractableGrabbing = null;

            Invoke("DelaySetFlag", 0.3f);
        }

        public void DelaySetFlag()
        {
            controller.IsGrabbing = false;
        }


    }
}
