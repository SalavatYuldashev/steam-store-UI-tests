using OpenQA.Selenium;

namespace task2.Pages;

public class StorePage(IWebDriver driver) : BasePage(driver)
{
    private By StorePageIndicator => By.XPath("//*[@id='store_header']//*[@id='store_nav_area']");

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