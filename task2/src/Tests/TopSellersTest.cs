using System;
using NUnit.Framework;
using OpenQA.Selenium;
using task2.Core.Browser;
using task2.Core.Utils;
using task2.Pages;

namespace task2.Tests;

[TestFixture]
public class TopSellersTestWithUniversalTestData
{
    private IWebDriver? _driver;
    private HomePage? _homePage;

    [SetUp]
    public void Setup()
    {
        _driver = WebDriverSingleton.Driver;
        _driver.Navigate().GoToUrl(AppConfig.Settings.BaseUrl);
        TestContext.WriteLine($"Navigated to: {AppConfig.Settings.BaseUrl}");

        _homePage = new HomePage(_driver);
        TestContext.WriteLine($"Page created: {_homePage}");
        Assert.That(_homePage.IsAt(), Is.True, "Home page was not opened.");
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            if (_driver != null)
            {
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
                /* ignore */
            }
        }
    }

    [Test]
    public void NavigateToTopSellers_ApplyFilters_FromJson()
    {
        var topSellersPage = _homePage!.GoToTopSellersPage(_driver!);
        Assert.That(topSellersPage.IsAt(), Is.True, "Top Sellers page was not opened.");
        topSellersPage.Cookies.AcceptIfPresent();
        TestContext.WriteLine("Top Sellers page opened.");


        var topSellersWithFiltersPage = topSellersPage.ScrollToMoreTopSellersButtonAndClick(_driver!);
        Assert.That(topSellersWithFiltersPage.IsAt(), Is.True, "Top Sellers with filters page was not opened.");
        topSellersPage.Cookies.AcceptIfPresent();
        TestContext.WriteLine("Top Sellers page with filters opened.");


        var cfg = FiltersReader.Load();
        var steps = cfg.TopSellersFilters;

        topSellersWithFiltersPage.FilterApplierComp.ApplyAllFilters(steps);
        TestContext.WriteLine($"Applied {steps.Count} filters.");


        var expected = topSellersWithFiltersPage.GetExpectedFilterResultsCount();
        var actual = topSellersWithFiltersPage.GetActualFilterResultsCount();
        Assert.That(actual, Is.EqualTo(expected),
            $"Mismatch: counter shows {expected}, list has {actual}.");


        var listGame = topSellersWithFiltersPage.GetFirstGameInfoFromList();
        Assert.That(listGame, Is.Not.Null);
        TestContext.WriteLine("Captured first game info from the list.");

        var detailsPage = topSellersWithFiltersPage.ClickFirstGameInList();
        Assert.That(detailsPage.IsAt(), Is.True, "Game details page was not opened.");
        TestContext.WriteLine("Opened game details page.");

        var detailsGame = detailsPage.GetGameInfo();

        Assert.That(listGame.Title, Is.EqualTo(detailsGame.Title), "Title mismatch.");
        Assert.That(listGame.ReleaseDate, Is.EqualTo(detailsGame.ReleaseDate), "Release date mismatch.");
        Assert.That(listGame.Price, Is.EqualTo(detailsGame.Price),
            $"Price mismatch: list {listGame.Price} vs details {detailsGame.Price}");
        TestContext.WriteLine("Verified game info matches between list and details.");
    }
}