using OpenQA.Selenium;
using task2.Pages.Components;

namespace task2.Pages;

public class GameDetailsPage: BasePage
{
    public GameDetailsPage(IWebDriver driver) : base(driver){}
    
    private By GameTitle => By.CssSelector(".apphub_AppName, .pageheader .page_title");
    private By GameReleaseDate => By.CssSelector(".release_date .date, .blockbg .block_content .date");
    private By GamePrice => By.CssSelector(".game_purchase_price, .game_area_purchase_game .price");

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