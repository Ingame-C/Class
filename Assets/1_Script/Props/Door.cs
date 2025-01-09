using System.Collections;
using UnityEngine;

namespace Class {

    public class Door : Usable
    {
        public override PropTypes PropType => PropTypes.Door;

        [SerializeField] private float doorOpenTime;


        private Vector3 originalPosition;
        private Vector3 openedPosition;

        private bool isOpened = false;
        private bool isLocked = false;               // 이건 자물쇠와 상호작용 해야함.
        private bool isCoroutineRunning = false;
        private Coroutine runningCoroutine = null;

        public Vector3 OriginalPosition { get => originalPosition; }

        private void Start()
        {
            originalPosition = transform.position;
            openedPosition = transform.TransformPoint(new Vector3(-transform.localScale.x, 0, 0));
        }

        public override void Interact(PlayerController controller)
        {
            if(isLocked)
            {
                // TODO : SOUND - 덜컹덜컹 or 잠겨있다는걸 알려줘야함
            }
            else
            {
                ToggleDoor();
            }
        }

        private void ToggleDoor()
        {
            // TODO : SOUND - 문 열리는 소리
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Door_open, 1.0f);
            if (isCoroutineRunning) StopCoroutine(runningCoroutine);

            if (isOpened)   runningCoroutine = StartCoroutine(CloseDoor());
            else            runningCoroutine = StartCoroutine(OpenDoor());
        }

        float elapsedTime = 0f;
        private IEnumerator OpenDoor()
        {
            isCoroutineRunning = true;
            isOpened = true;

            while (elapsedTime < doorOpenTime) {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originalPosition, openedPosition, elapsedTime / doorOpenTime);
                yield return null;
            }

            elapsedTime = doorOpenTime;
            isCoroutineRunning = false;
        }

        private IEnumerator CloseDoor()
        {
            isCoroutineRunning = true;
            isOpened = false;

            while (elapsedTime > 0)
            {
                elapsedTime -= Time.deltaTime;
                transform.position = Vector3.Lerp(originalPosition, openedPosition, elapsedTime / doorOpenTime);
                yield return null;
            }

            elapsedTime = 0f;
            isCoroutineRunning = false;
        }


    }
}
