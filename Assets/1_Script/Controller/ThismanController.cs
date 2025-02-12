using UnityEngine;

namespace Class
{
    // 경비 디스맨의 콘트롤러 입니다.
    // SetThismanTarget 함수에 player을 
    public class ThismanController : MonoBehaviour
    {
        private Transform target;

        private float traceSpeed = 3f;
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
            if (Vector3.SqrMagnitude(transform.position - target.position) < 6f) return;

            // TODO : SOUND - 달려갈때 발자국소리 점점 빨리나도록 하면 좋을 듯 합니다
            // + Thisman 속도조절 필요합니다. 교실 크기 정해지고나면 player와 거리 비례하게 구현하겠습니다.

            transform.position = MathFuncs.VectorLerp(transform.position, target.position, traceSpeed);
        }
    }

}