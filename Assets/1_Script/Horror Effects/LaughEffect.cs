using Class;

namespace Class
{
    /// <summary>
    /// 디스맨의 웃음소리 효과를 처리하는 클래스입니다.
    /// </summary>
    public class LaughEffect : HorrorEffect
    {
        public override EffectTypes EffectType { get => EffectTypes.Laugh; }

        public override void Activate()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Thisman_Laugh, 1.0f);
            GameManagerEx.Instance.IncreaseThismanSpawnProbability();
        }
    }
}