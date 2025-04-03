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
    
    
    private void Update()
    {
        HandleGameOver();
    }
    

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
        isVisible = (viewPos.x is > 0f and < 1f && viewPos.y is > 0f and < 1f && viewPos.z > 0f);
    }

    
    private void OnStandUp()
    {
        if (LecternManager.Instance.isToggleButtonsBeOn())
        {
            Destroy(this);
        }
        else
        {
            IsGameOver = true;
        }
    }


}
