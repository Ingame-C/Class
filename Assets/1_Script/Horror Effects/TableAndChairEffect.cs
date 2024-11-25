using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class;
using Unity.VisualScripting;
using UnityEngine;

public class TableAndChairEffect : HorrorEffect
{
    private EffectTypes effecttype = EffectTypes.TableAndChairEffect;
    public override EffectTypes EffectType { get => effecttype; }

    private GameObject desksParent = null;
    private GameObject chairsParent = null;

    private Desk[] desks;
    private Chair[] chairs;

    //효과의 타겟이 될 책상과 의자 List
    private List<Desk> deskTargeted;
    private List<Chair> chairTargeted;

    //원래 사이즈와 커지는 사이즈.
    private Vector3 originalSize = Vector3.one;
    [SerializeField, Range(1f, 10f)] private int scale;

    private List<int> randomNumberList = new List<int>();

    [SerializeField] private float enlargementTime;


    private void Awake()
    {
        #region initialization
        desks = null;
        chairs = null;
        desksParent = null;
        chairTargeted = null;
        deskTargeted = null;
        chairTargeted = null;
        randomNumberList = null;
        #endregion

        #region Get Desks and Chairs
        var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
        foreach (GameObject prop in initProps)
        {
            if (prop.name == "Tables") desksParent = prop;
            if (prop.name == "Chairs") chairsParent = prop;
        }

        if (desksParent == null || chairsParent == null)
        {
            Debug.LogError("There is no 'Tables or Chairs' object in Scene");
            return;
        }


        // chairs
        int counter = 0;
        foreach (Transform child in chairsParent.transform)
        {
            if (counter >= chairs.Count() || child.GetComponent<Chair>() == null)
            {
                Debug.LogError("Chair Binding Err : count mismatch or child doesn't have Chair");
                return;
            }
            chairs[counter++] = child.GetComponent<Chair>();
        }

        // desks
        counter = 0;
        foreach (Transform child in desksParent.transform)
        {
            if (counter >= desks.Count() || child.GetComponent<Desk>() == null)
            {
                Debug.LogError("Desk Binding Err : count mismatch or child doesn't have Desk");
                return;
            }
            desks[counter++] = child.GetComponent<Desk>();
        }
        #endregion

        #region Choose Arbitary 3 Desk and Chair

        int num = -1;
        for(var i = 0; i < 3; i++)
        {
            num = CreateUnDuplicateRandom(0, 20);
            deskTargeted.Add(desks[num]);
            chairTargeted.Add(chairs[num]);
        }
        #endregion

    }

    // Get Random Number, Do not allow duplication 
    int CreateUnDuplicateRandom(int start, int number)
    {
        int currentNumber = Random.Range(start, number);

        for (int i = 0; i < number;)
        {
            if (randomNumberList.Contains(currentNumber))
            {
                currentNumber = Random.Range(start, number);
            }
            else
            {
                randomNumberList.Add(currentNumber);
                i++;
            }
        }

        return currentNumber;
    }
    public override void Activate()
    {
        foreach(Desk prop in deskTargeted)
        {
            StartCoroutine(EnlargementObject(prop));
        }
        foreach (Chair prop in chairTargeted)
        {
            StartCoroutine(EnlargementObject(prop));
        }

    }



    #region Coroutine
    float elapsedTime = 0f;
    private IEnumerator EnlargementObject(Desk prop)
    {

        while (elapsedTime < enlargementTime)
        {
            elapsedTime += Time.deltaTime;
            prop.transform.localScale = Vector3.Lerp(originalSize, originalSize * scale, elapsedTime / enlargementTime);
            yield return null;
        }

        elapsedTime = enlargementTime;
    }

    private IEnumerator EnlargementObject(Chair prop)
    {

        while (elapsedTime < enlargementTime)
        {
            elapsedTime += Time.deltaTime;
            prop.transform.localScale = Vector3.Lerp(originalSize, originalSize * scale, elapsedTime / enlargementTime);
            yield return null;
        }

        elapsedTime = enlargementTime;
    }
    #endregion
}
