using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;
using UnityEngine.UIElements;

public class ApproachingWall : HorrorEffect
{
    private EffectTypes effecttype = EffectTypes.ApproachingWall;
    public override EffectTypes EffectType { get => effecttype; }

    // 0: Left, 1: Right, 2: Back, 3: Front
    private List<GameObject> walls = null;
    private List<Vector3> dir = new List<Vector3> 
    {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1) 
    };
    private List<Vector3> originalPos = new List<Vector3>
    {

    };

    private GameObject wallsParent = null;

    private float endTime = 50f;


    private void Awake()
    {

        #region Initialization
        walls = null;
        wallsParent = null;
        #endregion

        #region Get Walls
        var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
        foreach (GameObject prop in initProps)
        {
            if (prop.name == "Walls") wallsParent = prop;
        }

        if (wallsParent == null)
        {
            Debug.LogError("There is no 'Walls' object in Scene");
            return;
        }

        foreach (GameObject wall in walls)
        {
            walls.Add(wall);
        }

        #endregion;
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }


    float elapsedTime = 0;
    private IEnumerator SetWallApproach()
    {

        while (elapsedTime < endTime)
        {
            elapsedTime += Time.deltaTime;
            for(int i = 0; i < walls.Count; i++)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originalPosition, openedPosition, elapsedTime / endTime);
                yield return null;
            }
        }
    }
}
