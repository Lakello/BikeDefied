using IJunior.StateMachine;

public class ReloadSceneInState
{
    public void Load<TState>() where TState : State<GameStateMachine>
    {
        IJunior.TypedScenes.Game.Load<TState>();
    }
}