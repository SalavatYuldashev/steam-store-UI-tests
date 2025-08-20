using OpenQA.Selenium;

namespace task2.Pages;

public class AboutPage : BasePage
{
    private By AboutButton => By.Id("about");
    private By StoreButon => By.Id("storeButon");

    private int? ActivePlayers = null;
    private int? OnlinePlayers = null;

    public AboutPage(IWebDriver driver) : base(driver)
    {
    }

    public bool IsAt()
    {
        try
        {
            return Find(AboutButton).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public StorePage GoToStorePage(IWebDriver driver)
    {
        Find(StoreButon).Click();
        return new StorePage(driver);
    }

    public int GetActivePlayers()
    {
        return 10;
    }

    public int GetOnlinePlayers()
    {
        return 10;
    }
}