using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using task2.Pages.Components;

namespace task2.Pages;

public class TopSellersWithFiltersPage : BasePage
{
    public readonly SearchFilters SearchFilters;

    public TopSellersWithFiltersPage(IWebDriver driver) : base(driver)
    {
        SearchFilters = new SearchFilters(driver);
    }

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
        WaitResultsSettled();
        var text = Find(ExpectedFilterResultsCounter).Text;
        Console.WriteLine($"Ожидаемое количество игр текст:{text}");
        var onlynumber = Regex.Replace(text, @"[^\d]", "");
        Console.WriteLine($"Ожидаемые игры только цифры:{onlynumber}");
        var number = int.Parse(onlynumber);
        Console.WriteLine($"Ожилаемое кол-во в int:{number}");
        return number;
    }

    private void WaitResultsSettled(int timeoutSec = 10, int stableMs = 700)
    {
        var end = DateTime.UtcNow.AddSeconds(timeoutSec);
        string lastText = SafeGetText(ExpectedFilterResultsCounter);
        int lastRows = Driver.FindElements(FilterResultsBy).Count;
        var lastChange = DateTime.UtcNow;

        while (DateTime.UtcNow < end)
        {
            System.Threading.Thread.Sleep(150);

            string t = SafeGetText(ExpectedFilterResultsCounter);
            int r = Driver.FindElements(FilterResultsBy).Count;

            if (!string.Equals(t, lastText, StringComparison.Ordinal) || r != lastRows)
            {
                lastText = t;
                lastRows = r;
                lastChange = DateTime.UtcNow;
                continue;
            }

            if ((DateTime.UtcNow - lastChange).TotalMilliseconds >= stableMs && r > 0)
                return;
        }

        throw new WebDriverTimeoutException("Результаты поиска не успели стабилизироваться.");
    }

    private string SafeGetText(By by)
    {
        try
        {
            return Driver.FindElement(by).Text ?? string.Empty;
        }
        catch (NoSuchElementException)
        {
            return string.Empty;
        }
        catch (StaleElementReferenceException)
        {
            return string.Empty;
        }
    }

    public int GetActualFilterResultsCount()
    {
        WaitResultsSettled();

        var elements = Driver.FindElements(FilterResultsBy);
        var count = elements.Count;

        Console.WriteLine($"Фактическое количество игр на странице: {count}");
        return count;
    }

    public GameInfo GetFirstGameInfoFromList()
    {
        WaitResultsSettled();

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
}