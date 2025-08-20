using OpenQA.Selenium;

namespace task2.Pages;

public class StorePage : BasePage
{
    private By AboutButton => By.Id("storeButon");

    public StorePage(IWebDriver driver) : base(driver)
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
}