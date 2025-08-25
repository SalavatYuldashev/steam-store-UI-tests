using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using task2.Core.Utils;

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
                if (AppConfig.Settings.Headless)
                {
                    options.AddArgument("--headless");
                }

                if (AppConfig.Settings.Maximize)
                {
                    options.AddArgument("--start-maximized");
                }

                if (AppConfig.Settings.Incognito)
                {
                    options.AddArgument("--incognito");
                }

                options.AddArgument($"--lang={AppConfig.Settings.UiLanguage}");
                options.AddArgument("--enable-automation");

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(AppConfig.Settings.ImplicitWaitSec);
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