using NUnit.Framework;
using OpenQA.Selenium;
using task2.Core.Browser;
using task2.Core.Config;


namespace task2.Tests;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestContext.WriteLine($"Tests will run on website: {AppConfig.Settings.BaseUrl}");
        TestContext.WriteLine($"Browser settings: Headless= {AppConfig.Settings.Headless}, " +
                              $"Incognito={AppConfig.Settings.Incognito}, " +
                              $"Language={AppConfig.Settings.UILanguage}");
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        WebDriverSingleton.Quit();
    }
}