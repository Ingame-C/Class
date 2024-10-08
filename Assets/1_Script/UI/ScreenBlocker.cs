using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Class.UI
{

    public class ScreenBlocker : MonoBehaviour
    {
        [SerializeField] private Image blocker;

        public IEnumerator FadeInCoroutine(float duration)
        {
            blocker.color = Color.clear;
            blocker.gameObject.SetActive(true);

            float elapsedTime = 0f;
            Color originColor = blocker.color;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                blocker.color = Color.Lerp(originColor, Color.black, elapsedTime / duration);
            }
        }

        public IEnumerator FadeOutCoroutine(float duration)
        {

            float elapsedTime = 0f;
            Color originColor = blocker.color;

            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                blocker.color = Color.Lerp(originColor, Color.clear, elapsedTime / duration);
            }

            blocker.gameObject.SetActive(false);
        }

    }

}