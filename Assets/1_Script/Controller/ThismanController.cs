using UnityEngine;

namespace Class
{
    public class ThismanController : MonoBehaviour
    {
        private Transform target;

        bool isTargetSet = false;

        public void SetThismanTarget(Transform player)
        {
            target = player;
            isTargetSet = true;
        }

        private void Update()
        {
            if (!isTargetSet) return;

            transform.position = MathFuncs.VectorLerp(transform.position, target.position);
        }
    }

}