public static class GameLanguage
{
    private static string _value;

    public static string Value
    {
        get => _value;
        set
        {
            if (string.IsNullOrEmpty(_value))
                _value = value;
        }
    }
}