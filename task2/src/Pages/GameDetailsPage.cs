using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using task2.Pages.Components;

namespace task2.Pages;

public class GameDetailsPage(IWebDriver driver) : BasePage(driver)
{
    private By GameTitle => By.XPath("//*[@id='appHubAppName']");
    private By GameReleaseDate => By.XPath("//*[@class='release_date']//*[@class='date']");

    private By GamePrice => By.XPath("//*[@class='game_area_purchase_game']//*[@class='game_purchase_price price' " +
                                     "or @class='discount_final_price']");

    public bool IsAt()
    {
        try
        {
            return Find(GameTitle).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public GameInfo GetGameInfo()
    {
        var title = Text(GameTitle);
        var date = Text(GameReleaseDate);
        var rawPrice = Text(GamePrice);
        TestContext.WriteLine($"Raw prise from page {rawPrice}");

        var digits = new string(rawPrice.Where(char.IsDigit).ToArray());
        TestContext.WriteLine($"Digit prise from page {digits}");
        int price = string.IsNullOrEmpty(digits) ? 0 : int.Parse(digits);
        TestContext.WriteLine($"Int prise from page {price}");

        return new GameInfo { Title = title, ReleaseDate = date, Price = price };
    }
}