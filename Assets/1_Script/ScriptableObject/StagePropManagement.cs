using System;
using System.Collections;
using System.Collections.Generic;
using Class;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu (fileName = "StagePropManagement", menuName = "ScriptableObject/StagePropManagement", order = 1)]
public class StagePropManagement : ScriptableObject
{
    public List<GameObject> perStages;
    public Chair startingChair;
    public int currentStage;
    
    
    public void Initialize()
    {
        perStages = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            perStages.Add(null);
        }
        var initProps = GameObject.FindGameObjectsWithTag(Constants.TAG_INITPROPS);
        foreach (var props in initProps)
        {
            if(props.name == "Stage1Props") perStages[1] = props;
            if(props.name == "Stage2Props") perStages[2] = props;
            
        }
        var startChairVar = GameObject.FindGameObjectsWithTag(Constants.TAG_STARTINGCHAIR);
        startingChair = startChairVar[0].GetComponent<Chair>();
    }

    public void SetProps(int index)
    {
        for (int i = 0; i < perStages.Count; i++)
        {
            if (perStages[i] == null)
            {
                continue;
            }
            if (i == index)
            {
                Debug.Log(perStages[i].name);
                perStages[i].SetActive(true);
            }
            else
            {
                Debug.Log(perStages[i].name);
                perStages[i].SetActive(false);
            }
        }
    }
    

}
