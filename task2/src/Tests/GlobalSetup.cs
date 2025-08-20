using NUnit.Framework;
using OpenQA.Selenium;
using task2.Core.Browser;
using task2.Core.Config;

namespace task2.Tests;

[SetUpFixture]
public class GlobalSetup
{
    public static TestSettings Settings { get; set; } = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        Settings = ConfigReader.load();
        WebDriverSingleton.Configure(Settings);
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        WebDriverSingleton.Quit();
    }
}