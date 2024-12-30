using UnityEngine;

namespace Class
{
    // Horror Effect는 호러 이펙트들의 상위 클래스로 다형성을 구현하여 이펙트들을 구성하게 됩니다.
    public abstract class HorrorEffect: MonoBehaviour
    {
        // 이펙트 매니저에서 프리팹을 가져올 때, EffectTypes를 참고하여 가져오는 로직을 사용할 예정입니다.
        // 반드시 Enum을 확인해 주세요.
        public abstract EffectTypes EffectType { get; }

        protected virtual void Init()
        {

        }

        private void Awake()
        {
            Init();
        }


        // 각 이펙트마다 다르게 작성해주세요.
        // 정형적으로는 사운드가 발생한 뒤 플레이어를 억까하는 로직이 들어갈 수 있습니다.
        // ex: 유리가 깨지는 소리 이후, 모든 거울 사용 불가.
        public abstract void Activate();
    }
}