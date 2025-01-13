using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Hanzzz.MeshDemolisher;
using TMPro;
using UnityEngine;

public class MirrorBreakEffect : HorrorEffect
{
    public override EffectTypes EffectType => throw new System.NotImplementedException();
   
    [Space]
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private Transform breakPointsParent;
    [SerializeField] private Material interiorMaterial;

    [SerializeField] private KeyCode demolishKey;

    [SerializeField][Range(0f, 1f)] private float resultScale;
    [SerializeField] private Transform resultParent;

    private static MeshDemolisher meshDemolisher = new MeshDemolisher();

    private void Update()
    {
        if (!Input.GetKeyDown(demolishKey))
        {
            return;
        }

        if (targetGameObject.activeSelf)
        {
            Activate();
        }
    }
    public override void Activate()
    {
        SoundManager.Instance.CreateAudioSource(transform.position, SfxClipTypes.Mirror_Break, 1.0f);
        Enumerable.Range(0, resultParent.childCount).Select(i => resultParent.GetChild(i)).ToList().ForEach(x => DestroyImmediate(x.gameObject));
        List<Transform> breakPoints = Enumerable.Range(0, breakPointsParent.childCount).Select(x => breakPointsParent.GetChild(x)).ToList();

        var watch = System.Diagnostics.Stopwatch.StartNew();
        List<GameObject> res = meshDemolisher.Demolish(targetGameObject, breakPoints, interiorMaterial);
        watch.Stop();
        //logText.text = $"Demolish time: {watch.ElapsedMilliseconds}ms.";

        res.ForEach(x => x.transform.SetParent(resultParent, true));
        res.ForEach(x => x.AddComponent<BoxCollider>());
        res.ForEach(x => x.AddComponent<Rigidbody>());
        res.ForEach(x => x.GetComponent<Rigidbody>().AddForce(Vector3.right * 20, ForceMode.Impulse));
        res.ForEach(x => Destroy(x, 1));
        Enumerable.Range(0, resultParent.childCount).Select(i => resultParent.GetChild(i)).ToList().ForEach(x => x.localScale = resultScale * Vector3.one);

        targetGameObject.SetActive(false);
        StartCoroutine("Disappear", res);
    }

}
