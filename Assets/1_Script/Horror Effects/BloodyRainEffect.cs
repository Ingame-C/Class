using UnityEngine;

namespace Class 
{
    /// <summary>
    /// 피가 내리는 호러 효과를 구현하는 클래스입니다.
    /// 플레이어의 발자국 소리를 변경하고 바닥에 피 효과를 표시합니다.
    /// </summary>
    public class BloodyRainEffect : HorrorEffect
    {
        #region Properties
        public override EffectTypes EffectType { get => EffectTypes.BloodyRain; }
        #endregion

        #region Serialized Fields
        [Header("Effect Settings")]
        [SerializeField] private float duration;
        #endregion

        #region Public Methods
        /// <summary>
        /// 피가 내리는 효과를 활성화합니다.
        /// </summary>
        [ContextMenu("Activate")]
        public override void Activate()
        {
            ActivateBloodyRainEffect();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 피가 내리는 효과를 활성화하고 관련 설정을 적용합니다.
        /// </summary>
        private void ActivateBloodyRainEffect()
        {
            // TODO - 발자국 소리를 바꾸는 효과 필요
            GameManagerEx.Instance.Controller.SetBloodyRain(true);
            GameManagerEx.Instance.FloorController.ShowBlood(duration);
        }
        #endregion
    }
}