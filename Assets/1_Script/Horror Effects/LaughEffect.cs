using Class;

namespace Class
{
    public class LaughEffect : HorrorEffect
    {
        public override EffectTypes EffectType { get => EffectTypes.Laugh; }

        public override void Activate()
        {
            // TODO : SOUND - 디스맨 웃는 소리
            GameManagerEx.Instance.IncreaseThismanProb();
        }
    }
}