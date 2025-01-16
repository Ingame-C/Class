using Class;
using UnityEngine;

public class BreakMirrorEffect : HorrorEffect
{
    public override EffectTypes EffectType => throw new System.NotImplementedException();

    [SerializeField]
    MirrorBreakController[] m_Controllers;

    private void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            if (m_Controllers[i] == null)
            {
                m_Controllers[i] = GameObject.Find("Demolisher Script" + "_" + (i + 1))?.GetComponent<MirrorBreakController>();
            }
        }
    }

    [ContextMenu("Activate")]
    public override void Activate()
    {
        if (m_Controllers.Length == 0) { return; }

        SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Mirror_Break, 1.0f);
        for (int i = 0; i < 3; i++)
        {
            int j = UnityEngine.Random.Range(0, 5);
            m_Controllers[j].Demolish();
        }
    }
}