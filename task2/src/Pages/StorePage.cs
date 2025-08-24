using OpenQA.Selenium;
namespace task2.Pages;

public class StorePage : BasePage
{
    private By StorePageIndicator => By.XPath("//*[@id='store_header']//*[@id='store_nav_area']");

    public StorePage(IWebDriver driver) : base(driver)
    {
    }

    public bool IsAt()
    {
        try
        {
            return Find(StorePageIndicator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}