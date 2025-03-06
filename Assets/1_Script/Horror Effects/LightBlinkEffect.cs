using System.Collections;
using UnityEngine;

namespace Class
{
    /// <summary>
    /// 깜빡이는 조명 효과를 구현하는 클래스입니다.
    /// 조명 강도를 변화시키고 픽셀화 효과를 통해 호러 분위기를 연출합니다.
    /// </summary>
    public class LightBlinkEffect : HorrorEffect
    {
        #region Properties
        public override EffectTypes EffectType { get => EffectTypes.LightBlink; }
        #endregion

        #region Serialized Fields
        [Header("Effect Settings")]
        [SerializeField] private float fadeInDuration;
        #endregion

        #region Private Fields
        private PixelationEffect pixelize;
        private const float MAX_LIGHT_INTENSITY = 5.0f;
        private const float MIN_LIGHT_INTENSITY = 0f;
        private const float BLINK_DELAY = 0.4f;
        private const float DARKNESS_DELAY = 0.5f;
        #endregion

        #region Public Methods
        /// <summary>
        /// 깜빡이는 조명 효과를 활성화합니다.
        /// 카메라의 픽셀화 효과를 가져와서 효과를 시작합니다.
        /// </summary>
        public override void Activate()
        {
            InitializeComponents();
            StartCoroutine(LightBlinkCoroutine());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 필요한 컴포넌트들을 초기화합니다.
        /// </summary>
        private void InitializeComponents()
        {
            pixelize = Camera.main.GetComponent<PixelationEffect>();
        }

        /// <summary>
        /// 깜빡이는 효과를 실행하는 코루틴입니다.
        /// 조명 강도와 픽셀화 효과를 순차적으로 변화시킵니다.
        /// </summary>
        private IEnumerator LightBlinkCoroutine()
        {
            PlayBlinkSound();
            yield return ExecuteBlinkEffect();
            yield return ExecuteDarknessEffect();
            yield return ExecuteFadeInEffect();
        }

        /// <summary>
        /// 깜빡이는 사운드를 재생합니다.
        /// </summary>
        private void PlayBlinkSound()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Blink, 1.0f);
        }

        /// <summary>
        /// 깜빡이는 효과를 실행합니다.
        /// </summary>
        private IEnumerator ExecuteBlinkEffect()
        {
            GameManagerEx.Instance.SetLightIntensity(MAX_LIGHT_INTENSITY);
            yield return new WaitForSeconds(BLINK_DELAY);
            GameManagerEx.Instance.SetLightIntensity(MIN_LIGHT_INTENSITY);
        }

        /// <summary>
        /// 어두워지는 효과를 실행합니다.
        /// </summary>
        private IEnumerator ExecuteDarknessEffect()
        {
            pixelize.SetDarkness(MIN_LIGHT_INTENSITY);
            yield return new WaitForSeconds(DARKNESS_DELAY);
        }

        /// <summary>
        /// 점진적으로 밝아지는 효과를 실행합니다.
        /// </summary>
        private IEnumerator ExecuteFadeInEffect()
        {
            float elapsedTime = 0f;
            while(elapsedTime < fadeInDuration)
            {
                float progress = elapsedTime / fadeInDuration;
                pixelize.SetDarkness(progress);
                GameManagerEx.Instance.SetLightIntensity(progress);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        #endregion
    }
}