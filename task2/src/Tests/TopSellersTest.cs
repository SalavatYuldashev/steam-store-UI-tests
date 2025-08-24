using NUnit.Framework;
using OpenQA.Selenium;
using System;
using task2.Core.Browser;
using task2.Core.Utils;
using task2.Pages;

namespace task2.Tests;

[TestFixture]
[Category("ui")]
public class TopSellersTest
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
        try
        {
            if (_driver != null)
            {
                var currentUrl = _driver.Url;
                _driver.Navigate().GoToUrl(AppConfig.Settings.BaseUrl);
            }
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"TearDown failed: {ex.Message}");
            try
            {
                WebDriverSingleton.Quit();
            }
            catch
            {
                return;
            }
        }
    }

    [Test]
    public void NavigateToTopSellersAndApplyFilters()
    {
        var topSellersPage = _homePage.GoToTopSellersPage(_driver);
        Assert.That(topSellersPage.IsAt(), Is.True, "Failed to navigate to TopSellersPage");
        topSellersPage.AcceptCookiesIfPresent();
        TestContext.WriteLine("Открыта страница с лидерами продаж");

        var topSellersWithFiltersPage = topSellersPage.ScrollToMoreTopSellersButtonAndClick(_driver);
        Assert.That(topSellersWithFiltersPage.IsAt(), Is.True, "Failed to navigate to TopSellersWithFiltersPage");
        topSellersWithFiltersPage.AcceptCookiesIfPresent();
        TestContext.WriteLine("Открыта расширенная страница с лидерами продаж");
        topSellersWithFiltersPage.SearchFilters.SelectSteamOsLinuxCheckbox();
        TestContext.WriteLine("Выбран чек бокc SteamOsLinuxCheckbox");
        topSellersWithFiltersPage.SearchFilters.SelectCoopLanCheckbox();
        TestContext.WriteLine("Выбран чек бокc LAN Co-op");
        topSellersWithFiltersPage.SearchFilters.SelectActionCheckbox();
        TestContext.WriteLine("Выбран чек бокc Action");

        var expected = topSellersWithFiltersPage.GetExpectedFilterResultsCount();
        var actual = topSellersWithFiltersPage.GetActualFilterResultsCount();
        Assert.That(actual, Is.EqualTo(expected),
            $"Несовпадение: в счётчике {expected}, на странице {actual}");
        TestContext.WriteLine("Сравнили кол-во игр");

        var gameFromList = topSellersWithFiltersPage.GetFirstGameInfoFromList();
        Assert.That(gameFromList, Is.Not.Null);
        TestContext.WriteLine("Заполнили данные игры из списка");
        var gameDetailsPage = topSellersWithFiltersPage.ClickFirstGameInList();
        Assert.That(gameDetailsPage.IsAt(), Is.True, "Failed to navigate to FirstGameInList");
        TestContext.WriteLine("Открыли страницу первой игры");
        var gameFromGameDetailsPage = gameDetailsPage.GetGameInfo();
        Assert.That(gameFromList.Title, Is.EqualTo(gameFromGameDetailsPage.Title));
        Assert.That(gameFromList.ReleaseDate, Is.EqualTo(gameFromGameDetailsPage.ReleaseDate));
        Assert.That(gameFromList.Price, Is.EqualTo(gameFromGameDetailsPage.Price));
        TestContext.WriteLine("Сравнили информацию о играх");

    }
}