using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Class {
    /// <summary>
    /// 칠판에 힌트를 표시하는 컨트롤러입니다.
    /// 힌트의 페이드 인 효과와 위치를 관리합니다.
    /// </summary>
    [RequireComponent(typeof(GameObject))]
    public class BlackboardController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<GameObject> hints;        // 표시할 힌트 오브젝트들의 리스트
        [SerializeField] private GameObject blackBoard;         // 칠판 오브젝트
        #endregion

        #region Private Fields
        private Vector3 position;     // 힌트가 표시될 위치
        private Quaternion rotation;  // 힌트의 회전값
        private const float HINT_OFFSET = 0.1f;  // 힌트의 Z축 오프셋
        private const float FADE_DURATION = 1.0f;  // 페이드 인 효과 지속 시간
        #endregion

        #region Unity Methods
        private void Awake()
        {
            // 칠판의 초기 위치와 회전값을 저장
            rotation = blackBoard.transform.rotation;
            position = blackBoard.transform.position;
            position.z -= HINT_OFFSET;  // 힌트를 칠판 앞에 배치
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 지정된 인덱스의 힌트를 칠판에 표시합니다.
        /// </summary>
        /// <param name="hintIndex">표시할 힌트의 인덱스</param>
        public void WriteHintOnTheBlackBoard(int hintIndex)
        {
            if (hintIndex < 0 || hintIndex >= hints.Count)
            {
                Debug.LogWarning("힌트 인덱스가 유효하지 않습니다.");
                return;
            }

            GameObject hint = hints[hintIndex];
            hint.transform.position = position;
            hint.transform.rotation = rotation;

            StartCoroutine(FadeInHint(hint));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 힌트를 페이드 인 효과와 함께 표시합니다.
        /// </summary>
        /// <param name="hint">표시할 힌트 오브젝트</param>
        private IEnumerator FadeInHint(GameObject hint)
        {
            // CanvasGroup 컴포넌트 가져오기 또는 추가
            CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = hint.AddComponent<CanvasGroup>();
            }

            // 초기 설정
            hint.SetActive(true);
            canvasGroup.alpha = 0f;

            // 페이드 인 효과 실행
            float elapsedTime = 0f;
            SoundManager.Instance.CreateAudioSource(position, SfxClipTypes.Hint, 1.0f);

            while (elapsedTime < FADE_DURATION)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / FADE_DURATION);
                yield return null;
            }

            canvasGroup.alpha = 1f;  // 완전히 보이게 설정
        }
        #endregion
    }
}
