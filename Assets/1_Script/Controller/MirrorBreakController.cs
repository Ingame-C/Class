using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Hanzzz.MeshDemolisher;
using TMPro;
using UnityEngine;

public class MirrorBreakController : MonoBehaviour
{
   
    [Space]
    [SerializeField] private GameObject targetGameObject;
    [SerializeField] private Transform breakPointsParent;
    [SerializeField] private Material interiorMaterial;
    [SerializeField] private GameObject _camera;
    [SerializeField] private int index;

    [SerializeField][Range(0f, 1f)] private float resultScale;
    [SerializeField] private Transform resultParent;

    private static MeshDemolisher meshDemolisher = new MeshDemolisher();


    private Vector3 prevPosition;

    private void Start()
    {
        prevPosition = targetGameObject.transform.position;

        if(_camera == null)
        {
            _camera = GameObject.Find("Mirror Camera " + index);
        }
    }
    private void FixedUpdate()
    {
        Vector3 deltaPosition = targetGameObject.transform.position - prevPosition;
        if(deltaPosition.magnitude > 0f)
        {
            _camera.transform.position += deltaPosition;
        }
        prevPosition = targetGameObject.transform.position;

    }


    [ContextMenu("Demolish")]
    public void Demolish()
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
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        //Enumerable.Range(0,breakPointsParent.childCount).Select(i=>breakPointsParent.GetChild(i)).ToList().ForEach(x=>DestroyImmediate(x.gameObject));
        Enumerable.Range(0, resultParent.childCount).Select(i => resultParent.GetChild(i)).ToList().ForEach(x => DestroyImmediate(x.gameObject));

        targetGameObject.SetActive(true);
    }


}
