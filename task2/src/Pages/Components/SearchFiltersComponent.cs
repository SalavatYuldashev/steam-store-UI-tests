using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using task2.Core.Utils;

namespace task2.Pages.Components;

public class SearchFiltersComponent : BasePage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public SearchFiltersComponent(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(AppConfig.Settings.ImplicitWaitSec));
    }

    public By SteamOsLinuxCheckbox => By.XPath(
        "//span[contains(@class,'tab_filter_control') and contains(@data-loc,'SteamOS') and not(contains(@class,'tab_filter_control_not'))]");

    public By CoopLanCheckbox => By.XPath(
        "//span[contains(@class,'tab_filter_control') and contains(@data-loc,'LAN Co-op') " +
        "and not(contains(@class,'tab_filter_control_not'))]");

    public By ActionCheckbox =>
        By.XPath(
            "//span[contains(@class,'tab_filter_control') and @data-loc='Action' " +
            "and not(contains(@class,'tab_filter_control_not'))]");

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