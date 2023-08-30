using IJunior.StateMachine;

namespace IJunior.TypedScenes
{

    public interface ISceneLoadHandler<TMachine> where TMachine : StateMachine<TMachine>
    {
        void OnSceneLoaded<TState>() where TState : State<TMachine>;
    }
}
