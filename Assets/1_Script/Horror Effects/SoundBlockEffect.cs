using UnityEngine;

namespace Class
{
    public class SoundBlockEffect : HorrorEffect
    {
        public override EffectTypes EffectType => throw new System.NotImplementedException();

        public override void Activate()
        {
            SoundManager.Instance.BlockSound();
            GameManagerEx.Instance.TurnOnOffTV(false);
        }
    }
}