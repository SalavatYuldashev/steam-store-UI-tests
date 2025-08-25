using System;
using System.Threading;
using OpenQA.Selenium;

namespace task2.Core.Utils;

public static class UiWaits
{
    public static void WaitResultsSettled(
        IWebDriver driver,
        By counterBy,
        By rowsBy,
        int timeoutSec = 8,
        int stableMs = 450,
        bool throwOnTimeout = false)
    {
        var end = DateTime.UtcNow.AddSeconds(timeoutSec);
        string lastTxt = SafeGetText(driver, counterBy);
        int lastRows = driver.FindElements(rowsBy).Count;
        var lastChange = DateTime.UtcNow;

        while (DateTime.UtcNow < end)
        {
            Thread.Sleep(120);

            string t = SafeGetText(driver, counterBy);
            int r = driver.FindElements(rowsBy).Count;

            if (!string.Equals(t, lastTxt, StringComparison.Ordinal) || r != lastRows)
            {
                lastTxt = t;
                lastRows = r;
                lastChange = DateTime.UtcNow;
                continue;
            }

            if ((DateTime.UtcNow - lastChange).TotalMilliseconds >= stableMs)
                return;
        }

        if (throwOnTimeout)
            throw new WebDriverTimeoutException("Results did not settle in time.");
    }

    private static string SafeGetText(IWebDriver driver, By by)
    {
        try
        {
            return driver.FindElement(by).Text ?? string.Empty;
        }
        catch (NoSuchElementException)
        {
            return string.Empty;
        }
        catch (StaleElementReferenceException)
        {
            return string.Empty;
        }
    }
}