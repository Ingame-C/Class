using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Unity.VisualScripting;
using UnityEngine;

public class ArtToolReplicator : HorrorEffect
{

    [Header("Parents")]
    [Space]
    [SerializeField] private Transform artToolsParent;
    [SerializeField] private Transform replicasParent;

    [Header("Prefabs")]
    [Space]
    [SerializeField] private GameObject crayonReplica;
    [SerializeField] private GameObject coloredPenReplica;
    [SerializeField] private GameObject palletReplica;



    private EffectTypes effecttype = EffectTypes.ArtToolReplicator;
    public override EffectTypes EffectType { get => effecttype; }

    private void Start()
    {


    }

    private GameObject makeReplica(PropTypes propTypes)
    {
        if(propTypes == PropTypes.Crayons)
        {
            GameObject instance = Instantiate(crayonReplica);
            return instance;

        }
        else if (propTypes == PropTypes.Pallet)
        {
            GameObject instance = Instantiate(palletReplica);
            return instance;
        }
        else if (propTypes == PropTypes.ColoredPencil)
        {
            GameObject instance = Instantiate(coloredPenReplica);
            return instance;
        }
        return null;
    }

    public override void Activate()
    {
        var artTools = Enumerable.Range(0, artToolsParent.childCount).Select(i => artToolsParent.GetChild(i).gameObject);
        List<GameObject> replicas = new List<GameObject>();
        foreach (var tool in artTools)
        {
            if (tool.TryGetComponent(out ColoredPencil coloredPencil))
            {
                var copy = makeReplica(PropTypes.ColoredPencil);
                copy.transform.position = tool.transform.position;
                replicas.Add(copy);
                //Debug.Log("Detected! colored");
            }
            else if (tool.TryGetComponent(out Crayons crayons))
            {
                var copy = makeReplica(PropTypes.Crayons);
                copy.transform.position = tool.transform.position;
                replicas.Add(copy);
                //Debug.Log("Detected! crayon");
            }
            else if (tool.TryGetComponent(out Pallet pallet))
            {
                var copy = makeReplica(PropTypes.Pallet);
                copy.transform.position = tool.transform.position;
                replicas.Add(copy);
                //Debug.Log("Detected! pallet");
            }

        }

        replicas.ForEach(i => i.transform.SetParent(replicasParent));
    }


}
