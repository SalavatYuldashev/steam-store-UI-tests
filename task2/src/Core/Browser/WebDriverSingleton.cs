using System;
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
                if (Config.AppConfig.Settings.Headless)
                {
                    options.AddArgument("--headless");
                }

                if (Config.AppConfig.Settings.Maximize)
                {
                    options.AddArgument("--start-maximized");
                }

                if (Config.AppConfig.Settings.Incognito)
                {
                    options.AddArgument("--incognito");
                }

                options.AddArgument($"--lang={Config.AppConfig.Settings.UILanguage}");

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.AppConfig.Settings.ImplicitWaitSec);
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