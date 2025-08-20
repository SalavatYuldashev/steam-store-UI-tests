using Microsoft.Extensions.Configuration;

namespace task2.Core.Config;

public class ConfigReader
{
    public static TestSettings load()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var testSettings = new TestSettings();
        configurationRoot.Bind(testSettings);

        return testSettings;
    }
}