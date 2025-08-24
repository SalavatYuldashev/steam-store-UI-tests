using NUnit.Framework;
using OpenQA.Selenium;
using task2.Core.Browser;
using task2.Core.Utils;
using task2.Pages;

namespace task2.Tests;

[TestFixture]
[Category("ui")]
public class ActivePlayersLessThanOnlinePlayersTest
{
    private IWebDriver _driver = null!;
    private HomePage _homePage = null!;

    [SetUp]
    public void Setup()
    {
        _driver = WebDriverSingleton.Driver;
        _driver.Navigate().GoToUrl(AppConfig.Settings.BaseUrl);
        TestContext.WriteLine($"Выполнен переход на сайт: {AppConfig.Settings.BaseUrl}");
        _homePage = new HomePage(_driver);
        TestContext.WriteLine($"Создана страница: {_homePage.ToString()}");
        Assert.That(_homePage.IsAt(), Is.True, "Failed to navigate to HomePage in SetUp");
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Navigate().GoToUrl(AppConfig.Settings.BaseUrl);
    }

    [Test]
    public void ComparePlayers()
    {
        var aboutPage = _homePage.GoToAboutPage(_driver);
        Assert.That(aboutPage.IsAt(), Is.True, "Failed to navigate to AboutPage");
        TestContext.WriteLine($"Открыта страница: {_homePage.ToString()}");

        var activePlayers = aboutPage.GetActivePlayers();
        TestContext.WriteLine($"Играют сейчас: {activePlayers}");
        var onlinePlayers = aboutPage.GetOnlinePlayers();
        TestContext.WriteLine($"Онлайн: {onlinePlayers}");
        Assert.That(activePlayers < onlinePlayers, Is.True,
            $"Active players ({activePlayers}) must be greater than online players ({onlinePlayers})");

        var storePage = aboutPage.GoToStorePage(_driver);
        Assert.That(storePage.IsAt(), Is.True, "Failed to navigate to StorePage");
    }
}