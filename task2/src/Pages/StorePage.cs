using OpenQA.Selenium;

namespace task2.Pages;

public class StorePage : BasePage
{
    private By StorePageIndicator => By.CssSelector(".home_page_content");

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