using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace task2.Pages.Components;

public class CookieBannerComponent(IWebDriver driver)
{
    private readonly WebDriverWait _wait = new(driver, TimeSpan.FromSeconds(10));

    private readonly By _popup = By.Id("cookiePrefPopup");
    private readonly By _accept = By.Id("acceptAllButton");

    public void AcceptIfPresent()
    {
        try
        {
            var waitShort = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            waitShort.Until(d => d.FindElements(_popup).Count > 0);

            var btn = waitShort.Until(ExpectedConditions.ElementToBeClickable(_accept));
            new Actions(driver).MoveToElement(btn).Perform();
            btn.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.InvisibilityOfElementLocated(_popup));
        }
        catch (WebDriverTimeoutException)
        {
        }
        catch (ElementClickInterceptedException)
        {
            var btn = driver.FindElement(_accept);
            new Actions(driver).MoveToElement(btn).Perform();
            btn.Click();
        }
    }
}