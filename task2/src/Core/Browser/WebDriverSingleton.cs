using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using task2.Core.Config;

namespace task2.Core.Browser;

public sealed class WebDriverSingleton
{
    private static IWebDriver? _driver;

    public static IWebDriver Driver
    {
        get
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                if (TestSettings.Headless)
                {
                    options.AddArgument("--headless");
                }

                if (TestSettings.Maximize)
                {
                    options.AddArgument("--maximize");
                }

                if (TestSettings.Incognito)
                {
                    options.AddArgument("--incognito");
                }

                if (TestSettings.UILanguage.Equals("en"))
                {
                    options.AddArgument("--ui-language=en");
                }

                _driver = new ChromeDriver(options);
            }

            return _driver;
        }
    }

    public static void Quit()
    {
        _driver?.Quit();
        _driver?.Dispose();
        _driver = null;
    }
}