using System.Collections;
using System.Collections.Generic;
using Class;
using UnityEngine;

public class TestSmileMan : ISmileMan
{
    public override void HandleGameOver()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsGameOver = true;
        }
    }

    public override void GameOver()
    {
        Debug.Log("Game Over");
    }
}
