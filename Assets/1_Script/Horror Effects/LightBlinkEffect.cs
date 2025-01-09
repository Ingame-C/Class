using System.Collections;
using UnityEngine;

namespace Class
{
    public class LightBlinkEffect : HorrorEffect
    {
        [SerializeField] private float fadeInDuration;

        public override EffectTypes EffectType => throw new System.NotImplementedException();

        private PixelationEffect pixelize;

        [ContextMenu("Activation")]
        public override void Activate()
        {
            pixelize = Camera.main.GetComponent<PixelationEffect>();
            StartCoroutine(LightBlinkCoroutine());
        }

        private IEnumerator LightBlinkCoroutine()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Blink, 1.0f);
            // 스파크
            GameManagerEx.Instance.SetLightIntensity(5.0f);
            yield return new WaitForSeconds(0.4f);

            GameManagerEx.Instance.SetLightIntensity(0f);
            pixelize.SetDarkness(0f);
            yield return new WaitForSeconds(0.5f);

            float elapsedTime = 0f;
            while(elapsedTime < fadeInDuration)
            {
                pixelize.SetDarkness(elapsedTime / fadeInDuration);
                GameManagerEx.Instance.SetLightIntensity(elapsedTime / fadeInDuration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}