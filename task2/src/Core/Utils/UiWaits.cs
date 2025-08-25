using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace task2.Core.Utils
{
    public static class UiWaits
    {
        public static string SafeGetText(IWebDriver driver, By by)
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

        public static void WaitResultsSettled(
            IWebDriver driver,
            By counterBy,
            By rowsBy,
            int timeoutSec = 10,
            int stableMs = 700,
            int pollMs = 150)
        {
            var end = DateTime.UtcNow.AddSeconds(timeoutSec);

            string lastText = SafeGetText(driver, counterBy);
            int lastCount = driver.FindElements(rowsBy).Count;
            var lastChangeAt = DateTime.UtcNow;

            while (DateTime.UtcNow < end)
            {
                System.Threading.Thread.Sleep(pollMs);

                string t = SafeGetText(driver, counterBy);
                int c = driver.FindElements(rowsBy).Count;

                if (!string.Equals(t, lastText, StringComparison.Ordinal) || c != lastCount)
                {
                    lastText = t;
                    lastCount = c;
                    lastChangeAt = DateTime.UtcNow;
                    continue;
                }

                if ((DateTime.UtcNow - lastChangeAt).TotalMilliseconds >= stableMs && c > 0)
                    return;
            }

            throw new WebDriverTimeoutException("Результаты не успели стабилизироваться.");
        }

        public static void WaitSectionReady(IWebDriver driver, string blockKey, int timeoutSec = 6)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec));

            var box = By.XPath(
                $"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]");

            wait.Until(_ =>
            {
                try
                {
                    var el = driver.FindElement(box);
                    var cls = el.GetAttribute("class") ?? string.Empty;
                    return !cls.Contains("collapsed", StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });

            var innerVisible = By.XPath(
                $"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]//*[contains(@class,'block_content_inner') and not(contains(@style,'display: none'))]");
            wait.Until(ExpectedConditions.ElementExists(innerVisible));

            var anyItem = By.XPath(
                $"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]//*[contains(@class,'tab_filter_control_row') or contains(@class,'tab_filter_control_include')]");
            wait.Until(d => d.FindElements(anyItem).Count > 0);
        }
    }
}