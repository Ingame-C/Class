using System;
using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;

public class InFrontOfLectern : ISmileMan 
{
    #region private variables

    private bool isVisible = false;
    private float time = 0f;
    private PlayerController player;
    private LecternManager lecternManager;
    #endregion
    

    #region ISmileman implementation
    public override void HandleGameOver()
    {
        CheckIsVisible();
        
        if (!isVisible)
        {
            time = 0f;
            return;
        }
        
        time += Time.deltaTime;
        if (time > 3f)
        {
            IsGameOver = true;
        }
        
    }

    public override void GameOver()
    {
        base.GameOver();
        Debug.Log("Game Over!" + this.name);
    }
    #endregion
    
    #region Unity functions

    private void Start()
    {
        if (GameManagerEx.Instance.CurrentStage != 2) return;
        
        player = GameManagerEx.Instance.Controller;
        lecternManager = GameObject.Find("@LecternManager").GetComponent<LecternManager>();
        player.sitState.OnExit += OnStandUp;
    }

    private void OnDisable()
    {
        if (GameManagerEx.Instance.CurrentStage != 2) return;
        
        player.sitState.OnExit -= OnStandUp;
    }

    #endregion

    private void CheckIsVisible()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);
        isVisible = (viewPos.x > 0f && viewPos.x < 1f && viewPos.y > 0f && viewPos.y < 1f && viewPos.z > 0f);
    }

    
    /// <summary>
    ///  오엠알의 클리어 로직을 파악해야 함.
    /// </summary>
    private void OnStandUp()
    {
        lecternManager.CheckClear();
        
        if (lecternManager.IsClear)
        {
            // no op
            Destroy(this);
        }
        else
        {
            IsGameOver = true;
        }
    }
    
    
    
}
