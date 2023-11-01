namespace BikeDefied.TypedScenes
{
    public interface ISceneLoadHandlerOnArgument<T>
    {
        void OnSceneLoaded(T argument);
    }
}
