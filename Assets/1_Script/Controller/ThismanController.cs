using UnityEngine;

namespace Class
{
    /// <summary>
    /// 경비 디스맨의 동작을 제어하는 컨트롤러입니다.
    /// 플레이어를 추적하고 상호작용하는 기능을 담당합니다.
    /// </summary>
    public class ThismanController : MonoBehaviour
    {
        private const float DEFAULT_TRACE_SPEED = 3f;
        private const float MIN_DISTANCE_TO_TARGET = 6f;

        private Transform targetTransform;
        private float currentTraceSpeed;
        private bool isTargetSet;

        /// <summary>
        /// 디스맨의 추적 대상과 속도를 설정합니다.
        /// </summary>
        /// <param name="player">추적할 플레이어의 Transform</param>
        /// <param name="speed">추적 속도 (기본값: 5f)</param>
        public void SetThismanTarget(Transform player, float speed = 5f)
        {
            targetTransform = player;
            currentTraceSpeed = speed;
            isTargetSet = true;
            transform.LookAt(player);
        }

        private void Update()
        {
            if (!ShouldUpdateMovement()) return;
            UpdateMovement();
        }

        private bool ShouldUpdateMovement()
        {
            if (!isTargetSet) return false;
            return Vector3.SqrMagnitude(transform.position - targetTransform.position) >= MIN_DISTANCE_TO_TARGET;
        }

        private void UpdateMovement()
        {
            // TODO: 
            // 1. 발자국 소리 구현 - 속도에 따라 빈도 조절
            // 2. 교실 크기에 따른 속도 조절 로직 구현
            transform.position = MathFuncs.VectorLerp(transform.position, targetTransform.position, currentTraceSpeed);
        }
    }
}

