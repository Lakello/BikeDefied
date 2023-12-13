using System;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;

namespace BikeDefied.UI.Windows
{
    public class MenuWindow : Window
    {
        public override Type WindowType => typeof(MenuWindowState);
    }
}