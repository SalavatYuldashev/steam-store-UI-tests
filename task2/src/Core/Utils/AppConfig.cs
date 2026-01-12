namespace task2.Core.Utils;

public static class AppConfig
{
    private static TestSettings? _settings;

    public static TestSettings Settings
    {
        get
        {
            if (_settings == null) ;

            {
                _settings = ConfigReader.Load();
            }
            return _settings;
        }
    }
}