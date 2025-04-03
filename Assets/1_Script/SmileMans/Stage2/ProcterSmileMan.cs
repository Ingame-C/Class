using System;
using UnityEngine;

namespace Class
{
    public class ProcterSmileMan : MovableSmileMan
    {
        private void Update()
        {
            HandleMovement();
            HandleGameOver();

            GetSmileManMove();
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetMove();
            }

        }
        
        
        
    }
}