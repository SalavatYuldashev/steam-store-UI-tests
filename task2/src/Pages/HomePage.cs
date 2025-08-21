using OpenQA.Selenium;

namespace task2.Pages;

public class HomePage : BasePage
{
    public HomePage(IWebDriver driver) : base(driver)
    {
    }

    private By SteamLogo =>
        By.XPath("//*[@id='logo_holder']//a//img[contains(translate(@alt,'STEAM','steam'),'steam')]");

    private By AboutButton =>
        By.XPath("//*[contains(@class,'content')]//*[@class= 'supernav_container']//a[contains(.,'About')]");

    public bool IsAt()
    {
        try
        {
            return Find(SteamLogo).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public AboutPage GoToAboutPage(IWebDriver driver)
    {
        Click(AboutButton);
        return new AboutPage(driver);
    }
}