using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using task2.Core.Utils;

namespace task2.Pages.Components;

public class SearchFilters : BasePage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public SearchFilters(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(AppConfig.Settings.ImplicitWaitSec));
    }

    private By SteamOsLinuxCheckbox => By.XPath(
        "//*[contains(@class,'search_collapse_block')]//span[contains(@data-loc,'SteamOS')]");

    private By CoopLanCheckbox => By.XPath(
        "//*[contains(@class,'search_collapse_block')]//span[contains(@data-loc,'LAN Co-op')]");

    private By ActionCheckbox =>
        By.XPath(
            "//*[@id='TagFilter_Container']/*[contains(@data-loc,'Action') " +
            "and contains(@data-value,'19')]//*[@class='tab_filter_control_checkbox']");

    private By NumberOfPlayersExpandButton => By.XPath(
        "//*[@class='block_header']//*[contains(.,'number of player')]");

    private By TagSearchInput =>
        By.XPath("//*[@id='TagSuggest']");

    private By OperatingSystemHeader => By.XPath("//*[contains(@class,'block_header') ]//*[contains(text(),'OS')]");

    private By NumberOfPlayersHeader =>
        By.XPath("//*[contains(@class,'block_header') ]//*[contains(text(),'number')]/..");

    private By TagsHeader => By.XPath("//*[contains(@class,'block_header') ]//*[contains(text(),'tag')]");


    public void SelectSteamOsLinuxCheckbox() =>
        ClickWithExpand(SteamOsLinuxCheckbox, OperatingSystemHeader);


    public void SelectCoopLanCheckbox() =>
        ClickWithExpand(CoopLanCheckbox, NumberOfPlayersHeader);

    public void SelectActionCheckbox()
    {
        try
        {
            var searchInput = _wait.Until(ExpectedConditions.ElementToBeClickable(TagSearchInput));
            searchInput.Clear();
            searchInput.SendKeys("action");
            
            var checkbox = _wait.Until(ExpectedConditions.ElementToBeClickable(ActionCheckbox));
            checkbox.Click();
            System.Console.WriteLine(" Successfully applied Action filter");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($" Failed to apply Action filter: {ex.Message}");
        }
    }
}