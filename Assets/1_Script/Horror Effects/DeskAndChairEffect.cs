using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DeskAndChairEffect : HorrorEffect
{
    private EffectTypes effecttype = EffectTypes.DeskAndChairEffect;
    public override EffectTypes EffectType { get => effecttype; }


    private GameObject desksParent;
    private GameObject chairsParent;

    private Desk[] desks;
    private Chair[] chairs;

    //효과의 타겟이 될 책상과 의자 List
    private List<Desk> deskTargeted = new List<Desk>();
    private List<Chair> chairTargeted = new List<Chair>();

    [SerializeField] private int aNumberOfProps = 3;


    private void Start()
    {
        var go = GameObject.FindGameObjectsWithTag("InitProps");
        foreach(GameObject obj in go)
        {
            if(obj.name == "Desks")
            {
                desksParent = obj;
            }
            else if(obj.name == "Chairs")
            {
                chairsParent = obj;
            }
        }

        desks = desksParent.GetComponentsInChildren<Desk>();
        chairs = chairsParent.GetComponentsInChildren<Chair>();

        for(int i = 0; i < aNumberOfProps; i++)
        {
            int j = UnityEngine.Random.Range(0, 20);
            deskTargeted.Add(desks[j]);
            chairTargeted.Add(chairs[j]);
        }


        Activate();

    }


    public override void Activate()
    {
        StartCoroutine(EnlargementObject());
    }


    #region Coroutine
    [Header("Factor")]
    [SerializeField] float duration = 3.0f;
    [SerializeField] Vector3 targetSize;
    Vector3 originalSize = Vector3.one;

    float elapsedTime = 0f;

    private IEnumerator EnlargementObject()
    {

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            foreach (Chair chair in chairTargeted)
            {
                chair.transform.localScale = Vector3.Lerp(originalSize, targetSize, elapsedTime / duration);
            }
            foreach(Desk desk in deskTargeted)
            {
                desk.transform.localScale = Vector3.Lerp(originalSize, targetSize, elapsedTime / duration);
            }
            yield return null;
        }

        foreach (Chair chair in chairTargeted)
        {
            chair.transform.localScale = targetSize;
        }
        foreach (Desk desk in deskTargeted)
        {
            desk.transform.localScale = targetSize;
        }

    }
    #endregion
}
