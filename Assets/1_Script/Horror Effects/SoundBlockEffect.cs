using UnityEngine;

namespace Class
{
    /// <summary>
    /// 사운드를 차단하고 TV를 끄는 호러 효과를 구현하는 클래스입니다.
    /// 플레이어의 청각적 피드백을 제한하여 긴장감을 높입니다.
    /// </summary>
    public class SoundBlockEffect : HorrorEffect
    {
        #region Properties
        /// <summary>
        /// 이펙트의 타입을 반환합니다.
        /// </summary>
        public override EffectTypes EffectType => throw new System.NotImplementedException();
        // public override EffectTypes EffectType => EffectTypes.SoundBlock;
        #endregion

        #region Public Methods
        /// <summary>
        /// 사운드 차단 효과를 활성화합니다.
        /// 모든 사운드를 차단하고 TV를 끕니다.
        /// </summary>
        public override void Activate()
        {
            BlockAllSound();
            TurnOffTV();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 모든 사운드를 차단합니다.
        /// </summary>
        private void BlockAllSound()
        {
            SoundManager.Instance.BlockSound();
        }

        /// <summary>
        /// TV를 끕니다.
        /// </summary>
        private void TurnOffTV()
        {
            GameManagerEx.Instance.TurnOnOffTV(false);
        }
        #endregion
    }
}