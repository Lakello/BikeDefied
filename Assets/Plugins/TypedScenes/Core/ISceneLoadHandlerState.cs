using IJunior.StateMachine;

namespace IJunior.TypedScenes
{

    public interface ISceneLoadHandlerState<TMachine> where TMachine : StateMachine<TMachine>
    {
        void OnSceneLoaded<TState>() where TState : State<TMachine>;
    }

    public interface ISceneLoadHandler<T>
    {
        void OnSceneLoaded(T argument);
    }
}
