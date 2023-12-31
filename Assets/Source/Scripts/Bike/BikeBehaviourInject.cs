﻿using BikeDefied.Game;
using BikeDefied.InputSystem;

namespace BikeDefied.BikeSystem
{
    public struct BikeBehaviourInject
    {
        public BikeBehaviourInject(System.Func<(Player, Bike, IInputHandler)> inject) =>
            (Player, BikeBody, InputHandler) = inject();

        public Player Player { get; private set; }

        public Bike BikeBody { get; private set; }

        public IInputHandler InputHandler { get; private set; }
    }
}