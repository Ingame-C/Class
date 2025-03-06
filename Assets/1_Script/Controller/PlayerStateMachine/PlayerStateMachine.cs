using System;
using UnityEngine;

namespace Class.StateMachine
{
    /// <summary>
    /// 플레이어의 상태를 관리하는 상태 머신입니다.
    /// 상태 전환과 현재 상태의 업데이트를 담당합니다.
    /// </summary>
    public class PlayerStateMachine
    {
        #region State Variables
        private StateBase currentState;
        public StateBase CurrentState => currentState;
        #endregion

        #region State Change Methods
        public void Init(StateBase initialState)
        {
            currentState = initialState;
            currentState.Enter();
        }

        public void ChangeState(StateBase newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
        #endregion
    }
}