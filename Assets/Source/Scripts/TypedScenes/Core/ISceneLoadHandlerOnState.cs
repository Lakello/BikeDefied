using BikeDefied.FSM;

namespace BikeDefied.TypedScenes
{
    public interface ISceneLoadHandlerOnState<TMachine> where TMachine : StateMachine<TMachine>
    {
        void OnSceneLoaded<TState>(TMachine machine) where TState : State<TMachine>;
    }
}
