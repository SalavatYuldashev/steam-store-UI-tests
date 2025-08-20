using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using task2.Core.Config;

namespace task2.Core.Browser;

public sealed class WebDriverSingleton
{
    private static IWebDriver? _driver;
    private static TestSettings? _settings;

    public static IWebDriver Driver
    {
        get
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                if (_settings.Headless)
                {
                    options.AddArgument("--headless");
                }

                _driver = new ChromeDriver(options);
            }

            return _driver;
        }
    }

    public static void Configure(TestSettings testSettings)
    {
        _settings = testSettings;
    }

    public static void Quit()
    {
        _driver?.Quit();
        _driver?.Dispose();
        _driver = null;
    }
}