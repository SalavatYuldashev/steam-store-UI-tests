using System;
namespace task2.Core.Config;

public static class AppConfig
{
    private static TestSettings? _settings;

    public static TestSettings Settings
    {
        get
        {
            if (_settings == null) ;

            {
                _settings = ConfigReader.load();
            }
            return _settings;
        }
    }
}