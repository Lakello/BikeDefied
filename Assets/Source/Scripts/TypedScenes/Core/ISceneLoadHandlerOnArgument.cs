namespace BikeDefied.TypedScenes
{
    public interface ISceneLoadHandlerOnArgument<in TArgument>
    {
        void OnSceneLoaded(TArgument argument);
    }
}
