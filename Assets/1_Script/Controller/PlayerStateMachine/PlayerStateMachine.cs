using UnityEngine;

namespace Class.StateMachine
{
    public class PlayerStateMachine
    {
        private StateBase curState;
        public StateBase CurState { get=>curState;}

        public void Init(StateBase state)
        {
            curState = state;
            curState.Enter();
        }

        public void ChangeState(StateBase newState)
        {
            curState.Exit();

            curState = newState;    
            curState.Enter();
        }
    }

}