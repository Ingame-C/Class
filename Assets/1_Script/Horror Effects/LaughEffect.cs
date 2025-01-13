using Class;

namespace Class
{
    public class LaughEffect : HorrorEffect
    {
        public override EffectTypes EffectType { get => EffectTypes.Laugh; }

        public override void Activate()
        {
            SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Thisman_Laugh, 1.0f);
            GameManagerEx.Instance.IncreaseThismanProb();
        }
    }
}