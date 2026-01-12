using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using task2.Core.Utils;

namespace task2.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(AppConfig.Settings.ImplicitWaitSec));
    }

    protected IWebElement Find(By locator) => Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    protected void CustomClick(By locator) => Find(locator).Click();

    protected string Text(By locator) => Find(locator).Text;

    protected void HoverAndClick(By hoverLocator, By clickLocator)
    {
        var actions = new Actions(Driver);
        var hoverElement = Find(hoverLocator);
        actions.MoveToElement(hoverElement).Perform();

        var clickElement = Wait.Until(ExpectedConditions.ElementToBeClickable(clickLocator));
        clickElement.Click();
    }

    protected void ScrollAndClick(By locator)
    {
        var element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        var actions = new Actions(Driver);
        actions.MoveToElement(element).Perform();
        element.Click();
    }

    protected void ScrollTo(IWebElement el) =>
        new Actions(Driver).ScrollToElement(el).Perform();

    protected IWebElement WaitExists(By by) =>
        new WebDriverWait(Driver, TimeSpan.FromSeconds(AppConfig.Settings.ImplicitWaitSec))
            .Until(ExpectedConditions.ElementExists(by));
}