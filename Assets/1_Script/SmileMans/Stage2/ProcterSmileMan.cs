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

            if (!isMoving)
            {
                GetMove();
            }
        }
        
        
        #region ISmileman implementation
        public override void HandleGameOver()
        {
            var viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if(viewPos.x is > 0f and < 1f && viewPos.y is > 0f and < 1f && viewPos.z > 0f) IsGameOver = true;
        }

        public override void GameOver()
        {
            base.GameOver();
            Debug.Log("Game Over!" + this.name);
        }
        #endregion
        
        
    }
}