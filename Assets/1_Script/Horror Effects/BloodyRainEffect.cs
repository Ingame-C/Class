
using System.Collections;
using UnityEngine;

namespace Class {
    public class BloodyRainEffect :HorrorEffect
    {
        public override EffectTypes EffectType => throw new System.NotImplementedException();

        [SerializeField] private float duration;

        public override void Activate()
        {
            StartCoroutine(BloodyRainCoroutine());
        }

        private IEnumerator BloodyRainCoroutine()
        {
            GameManagerEx.Instance.Controller.SetBloodyRain(true);

            // TODO - 힌트를 지우거나 발자국 소리를 바꾸는 등의 추가적인 효과 필요

            yield return new WaitForSeconds(duration);
            GameManagerEx.Instance.Controller.SetBloodyRain(false);
        }

    }
}