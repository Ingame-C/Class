
using System;
using System.Collections.Generic;
using System.Linq;
using Class;
using UnityEngine;

public class SmileManManager : MonoBehaviour
{
    [Header("Smile Mans")]
    public List<ISmileMan> SmileMans = new List<ISmileMan>();

    private PlayerController player;
    
    private bool isGameOver;
    
    #region Unity methods
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        CheckAndExecuteGameOver();
    }

    private void CheckAndExecuteGameOver()
    {
        if (SmileMans.Exists(x => x.IsGameOver))
        {
            var satisfiedSmileMan = SmileMans.Find(x => x.IsGameOver);

            satisfiedSmileMan.GameOver();   // 게임 오버일 경우, 각 스마일 맨 고유의 게임 오버 로직을 실행.
            GameManagerEx.Instance.OnStageFailed(GameManagerEx.Instance.CurrentStage);
        }
    }

    #endregion
    
    #region private methods
    private void Init()
    {
        var smileMans = GameObject.FindGameObjectsWithTag("SmileMan");
        foreach (var smileMan in smileMans)
        {
            SmileMans.Add(smileMan.GetComponent<ISmileMan>());
        }
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    
    #endregion
    
}
