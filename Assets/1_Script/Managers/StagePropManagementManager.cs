using System;
using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;

public class StagePropManagementManager : MonoBehaviour
{

    [SerializeField] private StagePropManagement config;


    private void Awake()
    {
        config.Initialize();
    }

    void Start()
    {
        config.SetProps(GameManagerEx.Instance.CurrentStage);
    }
    
}
