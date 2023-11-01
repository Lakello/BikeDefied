using System;
using System.Collections.Generic;

namespace BikeDefied.LevelComponents
{
    public class LevelStateMachine
    {
        private List<LevelState> _states;

        private LevelState _currentState;

        public LevelStateMachine(Func<List<LevelState>> states)
        {
            _states = states();
        }

        public void EnterIn(int index)
        {
            if (index < 0 || index > _states.Count - 1)
                throw new ArgumentOutOfRangeException("index");

            _currentState?.Exit();
            _currentState = _states[index];
            _currentState.Enter();
        }
    }
}