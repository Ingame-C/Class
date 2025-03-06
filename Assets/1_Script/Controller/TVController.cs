using UnityEngine;

namespace Class
{
    /// <summary>
    /// TV 화면과 노이즈 효과를 관리하는 컨트롤러입니다.
    /// 플레이어와의 거리에 따라 노이즈 효과의 강도를 조절합니다.
    /// </summary>
    public class TVController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Screen References")]
        [SerializeField] private Transform tvScreen;
        [SerializeField] private Transform noiseScreen;
        [SerializeField] private Material tvNoiseMaterial;
        [SerializeField] private bool useScreen;

        [Header("Distance Settings")]
        [SerializeField, Range(1, 20)] private float minDistance;   // minDistance에서 alpha가 0
        [SerializeField, Range(1, 20)] private float maxDistance;   // maxDistance에서 alpha가 1

        [Header("Sound Settings")]
        [SerializeField] private float soundInterval = 120.0f;
        #endregion

        #region Private Fields
        private float timeValue;
        private float lastSoundTime = -Mathf.Infinity;
        #endregion

        #region Unity Methods
        private void Start()
        {
            OnOffTV(useScreen);
        }

        private void Update()
        {
            TVNoiseRender();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// TV 화면을 켜거나 끕니다.
        /// </summary>
        /// <param name="onoff">true면 TV를 켜고, false면 TV를 끕니다.</param>
        public void OnOffTV(bool onoff)
        {
            useScreen = onoff;
            SetScreenActive(onoff);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// TV 화면과 노이즈 화면의 활성화 상태를 설정합니다.
        /// </summary>
        private void SetScreenActive(bool active)
        {
            tvScreen.gameObject.SetActive(active);
            noiseScreen.gameObject.SetActive(active);
        }

        /// <summary>
        /// TV 노이즈 효과를 렌더링합니다.
        /// </summary>
        private void TVNoiseRender()
        { 
            if (!useScreen) return;

            UpdateNoiseTime();
            UpdateNoiseAlpha();
        }

        /// <summary>
        /// 노이즈 효과의 시간 값을 업데이트합니다.
        /// </summary>
        private void UpdateNoiseTime()
        {
            timeValue += Time.deltaTime * 10f;
            tvNoiseMaterial.SetFloat("_TimeValue", timeValue);
        }

        /// <summary>
        /// 플레이어와의 거리에 따라 노이즈 알파값을 업데이트합니다.
        /// </summary>
        private void UpdateNoiseAlpha()
        {
            float sqrDis = Vector3.Magnitude(transform.position - GameManagerEx.Instance.Controller.transform.position);
            float alpha = (sqrDis - minDistance) / (maxDistance - minDistance);
            SetNoiseAlpha(alpha);
        }

        /// <summary>
        /// 노이즈 효과의 알파값을 설정하고 사운드를 재생합니다.
        /// </summary>
        private void SetNoiseAlpha(float alpha)
        {
            PlayNoiseSound();
            alpha = Mathf.Clamp01(alpha);
            tvNoiseMaterial.SetFloat("_Alpha", alpha);
        }

        /// <summary>
        /// 일정 간격으로 노이즈 사운드를 재생합니다.
        /// </summary>
        private void PlayNoiseSound()
        {
            if (Time.time - lastSoundTime >= soundInterval)
            {
                SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.TV_Noise, 1.0f);
                lastSoundTime = Time.time;
            }
        }
        #endregion
    }
}