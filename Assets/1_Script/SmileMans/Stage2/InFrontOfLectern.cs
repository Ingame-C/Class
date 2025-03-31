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
    
    #endregion
    

    #region ISmileman implementation
    public override void HandleGameOver()
    {
        CheckIsVisible();
        
        if (!isVisible)
        {
            time = 0f;
            Debug.Log("Reset!");
            return;
        }
        
        time += Time.deltaTime;
        if (time > 3f)
        {
            IsGameOver = true;
            Debug.Log(time);
        }
        
    }

    public void GameOver()
    {
        base.GameOver();
        Debug.Log("Game Over!" + this.name);
    }
    #endregion

    private void CheckIsVisible()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);
        isVisible = (viewPos.x > 0f && viewPos.x < 1f && viewPos.y > 0f && viewPos.y < 1f && viewPos.z > 0f);
    }
}
