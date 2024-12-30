using System.Collections;
using System.Collections.Generic;
using Class;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ApproachingWall : HorrorEffect
{
    private EffectTypes effecttype = EffectTypes.ApproachingWall;
    public override EffectTypes EffectType { get => effecttype; }


    // 0: Left, 1: Right, 2: Back, 3: Front
    [SerializeField] private List<GameObject> walls = new List<GameObject>();
    private List<Vector3> dir = new List<Vector3> 
    {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1) 
    };

    [SerializeField] float speed = 3f;

    private GameObject wallsParent = null;

    private float endTime = 50f;


    private void Start()
    {

        #region Get Walls
        var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
        foreach (GameObject prop in initProps)
        {
            if (prop.name == "Walls")
            {
                wallsParent = prop;
                break;
            }
        }

        for(int i = 0; i < 2; i++)
        {
            walls.Add(wallsParent.transform.GetChild(i).gameObject);
        }


        #endregion;


        Activate();

    }

    public override void Activate()
    {
        StartCoroutine(SetWallApproach());
    }


    float elapsedTime = 0;
    private IEnumerator SetWallApproach()
    {
        for (int i = 0; i < 2; i++)
        {
            var wall = walls[i].GetComponentsInChildren<Transform>();
            foreach (var item in wall)
            {
                item.GameObject().tag = Constants.TAG_LAVAOBJECT;
            }
        }
        while (elapsedTime < endTime)
        {
            for (int i = 0; i < 2; i++)
            {
                walls[i].transform.position += dir[i] * speed * Time.deltaTime;
            }
            yield return null;
        }
        elapsedTime = 0;
    }
}


