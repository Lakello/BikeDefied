﻿using System;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;

namespace BikeDefied.UI.Windows
{
    public class LeaderboardWindow : Window
    {
        public override Type WindowType => typeof(LeaderboardWindowState);
    }
}