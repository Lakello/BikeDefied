public static class Check
{
    public static T IfNotNullThen<T>(object ifNotNull, System.Func<T> then)
    {
        if (ifNotNull != null)
            return then();

        return default;
    }

    public static void IfNotNullThen(object ifNotNull, System.Action then)
    {
        if (ifNotNull != null)
            then();
    }

    public static bool NotNull(object ifNotNull)
    {
        return ifNotNull != null;
    }
}