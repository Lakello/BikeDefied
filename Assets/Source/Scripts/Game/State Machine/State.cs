﻿namespace BikeDefied.FSM
{
    public abstract class State<TMachine>
        where TMachine : StateMachine<TMachine>
    {
        public abstract void Enter();

        public abstract void Exit();
    }
}