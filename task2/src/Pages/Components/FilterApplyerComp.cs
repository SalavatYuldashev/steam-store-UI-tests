using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using task2.Core.Utils;

namespace task2.Pages.Components
{
    public class FilterApplierComp(IWebDriver driver, By counterBy, By rowsBy) : BasePage(driver)
    {
        private readonly By _counterBy = counterBy;
        private readonly By _rowsBy = rowsBy;

        public By BuildBlockLocator(string blockKey) =>
            By.XPath($"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]");

        public By BuildBlockHeaderLocator(string blockKey) =>
            By.XPath(
                $"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]//*[contains(@class,'block_header')]");

        public By BuildFilterCheckboxLocator(string blockKey, string filterName)
        {
            var nameLit = XPathLiteral(filterName);
            return By.XPath(
                $"//*[contains(@class,'search_collapse_block') and contains(@data-collapse-name,'{blockKey}')]//*[@data-loc={nameLit}]//*[contains(@class,'checkbox')]");
        }

        public By? BuildSearchInputLocator(string blockKey) =>
            string.Equals(blockKey, "tag", StringComparison.OrdinalIgnoreCase) ? By.Id("TagSuggest") : null;

        private void ExpandIfCollapsed(string blockKey)
        {
            var box = WaitExists(BuildBlockLocator(blockKey));
            var cls = box.GetAttribute("class") ?? string.Empty;
            if (cls.Contains("collapsed", StringComparison.OrdinalIgnoreCase))
            {
                var header = Wait.Until(ExpectedConditions.ElementToBeClickable(BuildBlockHeaderLocator(blockKey)));
                header.Click();
                UiWaits.WaitResultsSettled(Driver, _counterBy, _rowsBy, throwOnTimeout: false);
            }
        }

        private void TypeSearchIfNeeded(string blockKey, bool useSearch, string text)
        {
            if (!useSearch) return;
            var inputBy = BuildSearchInputLocator(blockKey);
            if (inputBy == null) return;

            var input = Wait.Until(ExpectedConditions.ElementToBeClickable(inputBy));
            input.Clear();
            input.SendKeys(text);

            UiWaits.WaitResultsSettled(Driver, _counterBy, _rowsBy, throwOnTimeout: false);
        }

        private void ClickCheckbox(By checkboxBy)
        {
            var el = Wait.Until(ExpectedConditions.ElementToBeClickable(checkboxBy));
            ScrollTo(el);
            el.Click();
        }

        public bool IsChecked(By checkboxBy)
        {
            var cb = WaitExists(checkboxBy);
            var row = cb.FindElement(By.XPath("./ancestor::*[contains(@class,'tab_filter_control_row')][1]"));
            var rowCls = row.GetAttribute("class") ?? string.Empty;
            if (rowCls.Contains("checked", StringComparison.OrdinalIgnoreCase)) return true;

            var cls = cb.GetAttribute("class") ?? string.Empty;
            return cls.Contains("checked", StringComparison.OrdinalIgnoreCase);
        }

        public bool ApplyFilter(Filter step)
        {
            try
            {
                ExpandIfCollapsed(step.Block);

                if (step.UseSearch)
                {
                    var text = string.IsNullOrWhiteSpace(step.SearchText) ? step.Name : step.SearchText!;
                    TypeSearchIfNeeded(step.Block, true, text);
                }

                var checkboxBy = BuildFilterCheckboxLocator(step.Block, step.Name);
                ClickCheckbox(checkboxBy);
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                wait.Until(_ => IsChecked(checkboxBy));
                UiWaits.WaitResultsSettled(Driver, _counterBy, _rowsBy, throwOnTimeout: false);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ApplyFilter failed [{step.Block}:{step.Name}] → {ex.Message}");
                return false;
            }
        }

        public void ApplyAllFilters(IEnumerable<Filter> steps)
        {
            foreach (var s in steps)
            {
                var ok = ApplyFilter(s);
                if (!ok)
                    throw new InvalidOperationException($"Filter not applied: {s.Block}:{s.Name}");
            }
        }

        private static string XPathLiteral(string value)
        {
            if (!value.Contains("'")) return $"'{value}'";
            var parts = value.Split('\'');
            var concat = string.Join(", \"'\", ", parts.Select(p => $"'{p}'"));
            return $"concat({concat})";
        }
    }
}