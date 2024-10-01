using UnityEngine;

namespace Class
{
    public class ThismanController : MonoBehaviour
    {
        private Transform target;

        private float traceSpeed = 5f;
        private bool isTargetSet = false;


        public void SetThismanTarget(Transform player, float speed = 5f)
        {
            target = player;
            traceSpeed = speed;
            isTargetSet = true;

            transform.LookAt(player.transform);
        }

        private void Update()
        {
            if (!isTargetSet) return;
            if (Vector3.SqrMagnitude(transform.position - target.position) < 8f) return;

            transform.position = MathFuncs.VectorLerp(transform.position, target.position, traceSpeed);
        }
    }

}