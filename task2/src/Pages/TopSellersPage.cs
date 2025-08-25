using OpenQA.Selenium;
using task2.Pages.Components;

namespace task2.Pages;

public class TopSellersPage(IWebDriver driver) : BasePage(driver)
{
    public readonly CookieBannerComponent Cookies = new(driver);

    private By TopSellersPageIndicator =>
        By.XPath("//*[contains(@data-featuretarget,'react-root')]//*[contains(text(),'Top Sellers')]");

    private By FirstGameResult =>
        By.XPath("//*[@id='search_resultsRows']//a[contains(@class,'search_result_row')][1]");

    private By ViewMoreTopSellersButton =>
        By.XPath("//button[contains(@class,'DialogButton')]");


    public bool IsAt()
    {
        try
        {
            return Find(TopSellersPageIndicator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    public TopSellersWithFiltersPage ScrollToMoreTopSellersButtonAndClick(IWebDriver driver)
    {
        ScrollAndClick(ViewMoreTopSellersButton);
        return new TopSellersWithFiltersPage(driver);
    }
}