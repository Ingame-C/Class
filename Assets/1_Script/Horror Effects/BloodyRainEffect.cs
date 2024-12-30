using UnityEngine;

namespace Class {
    public class BloodyRainEffect :HorrorEffect
    {
        public override EffectTypes EffectType => throw new System.NotImplementedException();

        [SerializeField] private float duration;

        [ContextMenu("Activate")]
        public override void Activate()
        {
            // TODO - 발자국 소리를 바꾸는 효과 필요
            GameManagerEx.Instance.Controller.SetBloodyRain(true);
            GameManagerEx.Instance.FloorController.ShowBlood(duration);
        }

    }
}