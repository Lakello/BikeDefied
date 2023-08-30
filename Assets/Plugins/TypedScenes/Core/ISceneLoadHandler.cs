namespace IJunior.TypedScenes
{
    using IJunior.StateMachine;

    public interface ISceneLoadHandler<TMachine> where TMachine : StateMachine<TMachine>
    {
        void OnSceneLoaded<TState>() where TState : State<TMachine>;
    }
}
