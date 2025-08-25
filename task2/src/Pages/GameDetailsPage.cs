using OpenQA.Selenium;
using task2.Pages.Components;

namespace task2.Pages;

public class GameDetailsPage(IWebDriver driver) : BasePage(driver)
{
    private By GameTitle => By.XPath("//*[@id='appHubAppName']");
    private By GameReleaseDate => By.XPath("//*[@class='release_date']//*[@class='date']");
    private By GamePrice => By.XPath("//*[@class='game_area_purchase_game']//*[@class='game_purchase_price price']");

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
        return new GameInfo
        {
            Title = Text(GameTitle),
            ReleaseDate = Text(GameReleaseDate),
            Price = Text(GamePrice)
        };
    }
}