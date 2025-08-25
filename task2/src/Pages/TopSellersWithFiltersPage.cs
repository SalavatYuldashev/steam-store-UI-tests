using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using task2.Core.Utils;
using task2.Pages.Components;

namespace task2.Pages;

public class TopSellersWithFiltersPage : BasePage
{
    public readonly SearchFiltersComponent SearchFiltersComponent;
    public readonly FilterApplierComp FilterApplierComp;
    public readonly CookieBannerComponent Cookies;


    public TopSellersWithFiltersPage(IWebDriver driver) : base(driver)
    {
        SearchFiltersComponent = new SearchFiltersComponent(driver);
        FilterApplierComp = new FilterApplierComp(driver, _counterBy, _rowsBy);
        Cookies = new CookieBannerComponent(driver);
    }

    private readonly By _counterBy = By.XPath("//*[@class='search_results_count']");
    private readonly By _rowsBy = By.XPath("//a[contains(@class,'search_result_row')]");

    private By TopSellersMoreResultsPageIndicator =>
        By.XPath("//*[contains(@class,'pageheader') and (contains(text(),'Top Sellers'))]");


    private By FirstGameResult =>
        By.XPath("//*[@id='search_resultsRows']//a[contains(@class,'search_result_row')][1]");

    private By FirstGameTitle =>
        By.XPath(
            "//*[@id='search_resultsRows']//a[contains(@class,'search_result_row')][1]//*[contains(@class,'title')]");

    private By FirstGameReleaseDate =>
        By.XPath(
            "//*[@id='search_resultsRows']//a[contains(@class,'search_result_row')][1]//*[contains(@class,'search_released')]");

    private By FirstGamePrice =>
        By.XPath(
            "//*[@id='search_resultsRows']//a[contains(@class,'search_result_row')][1]//*[contains(@class,'search_price')]");


    private By ExpectedFilterResultsCounter => By.XPath("//*[@class='search_results_count']");
    private By FilterResultsBy => By.XPath("//a[contains(@class,'search_result_row')]");


    public bool IsAt()
    {
        try
        {
            return Find(TopSellersMoreResultsPageIndicator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }


    public int GetExpectedFilterResultsCount()
    {
        UiWaits.WaitResultsSettled(Driver, ExpectedFilterResultsCounter, FilterResultsBy);
        var text = Find(ExpectedFilterResultsCounter).Text;
        var onlyNumber = Regex.Replace(text, @"[^\d]", "");
        var number = int.Parse(onlyNumber);
        return number;
    }

    public int GetActualFilterResultsCount()
    {
        UiWaits.WaitResultsSettled(Driver, ExpectedFilterResultsCounter, FilterResultsBy);
        var elements = Driver.FindElements(FilterResultsBy);
        var count = elements.Count;
        return count;
    }

    public GameInfo GetFirstGameInfoFromList()
    {
        UiWaits.WaitResultsSettled(Driver, ExpectedFilterResultsCounter, FilterResultsBy);

        var title = Text(FirstGameTitle);
        var date = Text(FirstGameReleaseDate);
        var rawPrice = Text(FirstGamePrice);
        var price = rawPrice
            .Split('\n', '\r')
            .Select(s => s.Trim())
            .LastOrDefault(s => !string.IsNullOrWhiteSpace(s)) ?? string.Empty;

        return new GameInfo { Title = title, ReleaseDate = date, Price = price };
    }

    public GameDetailsPage ClickFirstGameInList()
    {
        var firstGame = Find(FirstGameResult);
        firstGame.Click();
        return new GameDetailsPage(Driver);
    }

    public bool IsCheckboxChecked(By locator)
    {
        System.Threading.Thread.Sleep(150);
        try
        {
            var element = Find(locator);
            var classAttr = element.GetAttribute("class") ?? string.Empty;
            return classAttr.Contains("checked", StringComparison.OrdinalIgnoreCase);
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}