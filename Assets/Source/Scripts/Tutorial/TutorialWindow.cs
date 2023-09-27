using IJunior.StateMachine;
using System;

public class TutorialWindow : Window
{
    private void OnEnable()
    {
        
    }

    public override Type WindowType => typeof(MenuWindowState);
}