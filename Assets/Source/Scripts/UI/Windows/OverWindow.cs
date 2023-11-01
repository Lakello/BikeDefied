using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using System;

namespace BikeDefied.UI.Windows
{
    public class OverWindow : Window
    {
        public override Type WindowType => typeof(OverWindowState);
    }
}