using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace task2.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    private readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    protected IWebElement Find(By locator) => Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    protected void Click(By locator) => Find(locator).Click();

    protected void Type(By locator, string text)
    {
        var element = Find(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string Text(By locator) => Find(locator).Text;
}