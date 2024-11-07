using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class {
    [RequireComponent(typeof(GameObject))]
    public class BlackboardController : MonoBehaviour
    {
        // Stage 별로 힌트 파일 하나씩 입력해 주세요.
        [SerializeField] private List<GameObject> hints;
        [SerializeField] private GameObject blackBoard;

        private Vector3 position;
        private Quaternion rotation;

        private void Awake()
        {
            // Blackboard의 위치와 회전을 저장합니다.
            rotation = blackBoard.transform.rotation;
            position = blackBoard.transform.position;

            position.z -= 0.1f;

        }

        public void WriteHintOnTheBlackBoard(int hintIndex)
        {
            if (hintIndex < 0 || hintIndex >= hints.Count)
            {
                Debug.Log("Hint Index Error.");
                return;
            }

            GameObject hint = hints[hintIndex];
            hint.transform.position = position;
            hint.transform.rotation = rotation;

            StartCoroutine(FadeInHint(hint));
        }

        private IEnumerator FadeInHint(GameObject hint)
        {
            CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = hint.AddComponent<CanvasGroup>();
            }

            hint.SetActive(true);
            canvasGroup.alpha = 0f;

            float fadeDuration = 1.0f;  // Fade In 시간
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); 
                yield return null;
            }

            canvasGroup.alpha = 1f;  // 완전히 보이게 설정
        }
    }

}
