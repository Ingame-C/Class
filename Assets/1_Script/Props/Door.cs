using System.Collections;
using UnityEngine;

namespace Class {
    /// <summary>
    /// 문 오브젝트의 동작을 제어하는 클래스입니다.
    /// 문의 열기/닫기 애니메이션과 상호작용을 처리합니다.
    /// </summary>
    public class Door : Usable
    {
        #region Properties
        public override PropTypes PropType => PropTypes.Door;
        public Vector3 OriginalPosition => originalPosition;
        #endregion

        #region Serialized Fields
        [SerializeField] private float doorOpenTime = 1f;  // 문이 열리고 닫히는 데 걸리는 시간
        #endregion

        #region Private Fields
        private Vector3 originalPosition;    // 문의 초기 위치
        private Vector3 openedPosition;      // 문이 열린 상태의 위치
        private float elapsedTime = 0f;      // 애니메이션 진행 시간
        private bool isOpened = false;       // 문이 열린 상태인지 여부
        private bool isLocked = false;       // 문이 잠겨있는지 여부
        private bool isCoroutineRunning = false;  // 문 애니메이션이 진행 중인지 여부
        private Coroutine runningCoroutine = null;  // 현재 실행 중인 코루틴
        #endregion

        #region Unity Methods
        private void Start()
        {
            // 문의 초기 위치와 열린 상태의 위치를 설정
            originalPosition = transform.position;
            openedPosition = transform.TransformPoint(new Vector3(-transform.localScale.x, 0, 0));
        }
        #endregion

        #region Interaction
        /// <summary>
        /// 플레이어가 문과 상호작용할 때 호출됩니다.
        /// 문이 잠겨있거나 애니메이션 중이면 동작하지 않습니다.
        /// </summary>
        public override void Interact(PlayerController controller)
        {
            if (isCoroutineRunning) return;

            if (isLocked)
            {
                // TODO: 잠긴 문 소리 효과 추가
                // SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Door_locked, 1.0f);
            }
            else
            {
                ToggleDoor();
            }
        }
        #endregion

        #region Door Control
        /// <summary>
        /// 문의 상태를 토글합니다. 열려있으면 닫고, 닫혀있으면 엽니다.
        /// </summary>
        private void ToggleDoor()
        {
            if (isOpened)
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Door_open, 1.0f);
                runningCoroutine = StartCoroutine(CloseDoor());
            }
            else
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Door_open, 1.0f);
                runningCoroutine = StartCoroutine(OpenDoor());
            }
        }

        /// <summary>
        /// 문을 여는 애니메이션을 실행합니다.
        /// </summary>
        private IEnumerator OpenDoor()
        {
            isCoroutineRunning = true;
            elapsedTime = 0f;

            while (elapsedTime < doorOpenTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / doorOpenTime;
                transform.position = Vector3.Lerp(originalPosition, openedPosition, t);
                yield return null;
            }

            transform.position = openedPosition;
            isOpened = true;
            isCoroutineRunning = false;
        }

        /// <summary>
        /// 문을 닫는 애니메이션을 실행합니다.
        /// </summary>
        private IEnumerator CloseDoor()
        {
            isCoroutineRunning = true;
            elapsedTime = doorOpenTime;

            while (elapsedTime > 0)
            {
                elapsedTime -= Time.deltaTime;
                float t = elapsedTime / doorOpenTime;
                transform.position = Vector3.Lerp(originalPosition, openedPosition, t);
                yield return null;
            }

            transform.position = originalPosition;
            isOpened = false;
            isCoroutineRunning = false;
        }
        #endregion
    }
}
