using System;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;

namespace BikeDefied.UI.Windows
{
    public class PlayWindow : Window
    {
        public override Type WindowType => typeof(PlayWindowState);
    }
}