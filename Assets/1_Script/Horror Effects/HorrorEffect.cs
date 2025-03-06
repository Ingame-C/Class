using UnityEngine;

namespace Class
{
    /// <summary>
    /// 호러 이펙트들의 기본 클래스입니다.
    /// 모든 호러 이펙트는 이 클래스를 상속받아 구현됩니다.
    /// </summary>
    public abstract class HorrorEffect : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// 이펙트의 타입을 나타내는 프로퍼티입니다.
        /// 이펙트 매니저에서 프리팹을 가져올 때 참조됩니다.
        /// </summary>
        public abstract EffectTypes EffectType { get; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Init();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 이펙트의 초기화를 수행하는 가상 메서드입니다.
        /// 자식 클래스에서 필요한 초기화 로직을 구현할 수 있습니다.
        /// </summary>
        protected virtual void Init()
        {
            // 기본 구현은 비어있습니다.
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 이펙트를 활성화하는 추상 메서드입니다.
        /// 각 이펙트마다 고유한 활성화 로직을 구현해야 합니다.
        /// 일반적으로 사운드 재생 후 플레이어에게 영향을 주는 로직이 포함됩니다.
        /// 예시: 유리가 깨지는 소리 이후 모든 거울 사용 불가
        /// </summary>
        public abstract void Activate();
        #endregion
    }
}