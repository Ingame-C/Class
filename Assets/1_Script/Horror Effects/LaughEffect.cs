using Class;

namespace Class
{
    public class LaughEffect : HorrorEffect
    {
        public override EffectTypes EffectType => throw new System.NotImplementedException();

        public override void Activate()
        {
            // TODO : SOUND - 디스맨 웃는 소리
            GameManagerEx.Instance.IncreaseThismanProb();
        }
    }
}