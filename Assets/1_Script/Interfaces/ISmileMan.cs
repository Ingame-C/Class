using System;
using UnityEngine;

namespace Class
{
    public abstract class ISmileMan : MonoBehaviour
    {
        public bool IsGameOver { get; protected set; } = false;
        
        /// <summary>
        /// 게임 오버 조건을 충족하는 지, 계속 체크하는 함수.
        /// 게임 오버 조건 충족 시, IsGameOver를 true로 지정하면 됨.
        /// </summary>
        public abstract void HandleGameOver();
        
        /// <summary>
        /// 게임 오버인 경우, 각 고유한 게임 오버 로직을 실행시키는 함수.
        /// </summary>
        public abstract void GameOver();

        private void Update()
        {
            HandleGameOver();
        }
    }
}