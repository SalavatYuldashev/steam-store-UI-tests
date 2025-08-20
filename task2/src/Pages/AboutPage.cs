using OpenQA.Selenium;

namespace task2.Pages;

public class AboutPage : BasePage
{
    private By AboutPageIndicator => By.CssSelector(".about-header");
    private By StoreButton => By.CssSelector("a[href*='store']");
    private By OnlinePlayersElement => By.XPath("//[contains(., 'online')]");
    private By ActivePlayersElement => By.XPath("//[contains(., 'playing now')]");
    private int? ActivePlayers = null;
    private int? OnlinePlayers = null;

    public AboutPage(IWebDriver driver) : base(driver)
    {
    }

    public bool IsAt()
    {
        try
        {
            return Find(AboutPageIndicator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public StorePage GoToStorePage(IWebDriver driver)
    {
        Find(StoreButton).Click();
        return new StorePage(driver);
    }

    public int GetActivePlayers()
    {
        try
        {
            var text = Find(ActivePlayersElement).Text;
            var numberString = text.Replace("playing now: ", "").Replace(",", "");
            return int.Parse(numberString);

        }
        catch
        {
            return 0;
        }
    }

    public int GetOnlinePlayers()
    {
        try
        {
            var text = Find(ActivePlayersElement).Text;
            var numberString = text.Replace("online: ", "").Replace(",", "");
            return int.Parse(numberString);

        }
        catch
        {
            return 0;
        }
    }
}