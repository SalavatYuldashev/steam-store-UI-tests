using Microsoft.Extensions.Configuration;

namespace task2.Core.Utils;

public abstract class ConfigReader
{
    public static TestSettings Load()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("src/Core/Config/appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var testSettings = new TestSettings();
        configurationRoot.Bind(testSettings);

        return testSettings;
    }
}