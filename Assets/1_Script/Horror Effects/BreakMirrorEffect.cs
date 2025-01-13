using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;

public class BreakMirrorEffect : HorrorEffect
{
    public override EffectTypes EffectType => throw new System.NotImplementedException();

    [SerializeField]
    MirrorBreakController[] m_Controllers;

    private void Start()
    {
        
    }

    [ContextMenu("Activate")]
    public override void Activate()
    {
        if(m_Controllers.Length == 0) { return; }

        for (int i = 0; i < 3; i++)
        {
            int j = UnityEngine.Random.Range(0, 5);
            m_Controllers[i].Demolish();
        }
    }
}
