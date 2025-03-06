using System.Collections;
using UnityEngine;

namespace Class {
    /// <summary>
    /// 바닥에 핏자국 효과를 표시하는 컨트롤러입니다.
    /// 지정된 시간 동안 페이드 인 효과로 핏자국을 표시합니다.
    /// </summary>
    public class BloodyFloorController : MonoBehaviour
    {
        #region Public Methods
        /// <summary>
        /// 지정된 시간 동안 핏자국을 표시합니다.
        /// </summary>
        /// <param name="duration">핏자국이 표시될 시간(초)</param>
        public void ShowBlood(float duration)
        {
            StartCoroutine(ShowBloodCoroutine(duration));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 핏자국을 페이드 인 효과와 함께 표시하는 코루틴입니다.
        /// </summary>
        /// <param name="duration">페이드 인에 걸릴 시간(초)</param>
        private IEnumerator ShowBloodCoroutine(float duration)
        {
            // 모든 자식 오브젝트 초기화
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material.color = Color.clear;
                child.gameObject.SetActive(true);
            }

            // 페이드 인 효과 실행
            float elapsedTime = 0f;
            Color tmpColor = Color.white;

            while (elapsedTime < duration)
            {
                tmpColor.a = elapsedTime / duration;
                foreach (Transform child in transform)
                {
                    child.GetComponent<MeshRenderer>().material.color = tmpColor;
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }
        #endregion
    }
}
