using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace task2.Pages;

public class AboutPage(IWebDriver driver) : BasePage(driver)
{
    private By AboutPageIndicator => By.XPath("//*[contains(@id,'about_') and contains(@class,'about_')]");

    private By StoreButton =>
        By.XPath("//*[@id='global_header']//a[@class='menuitem supernav' and normalize-space()='STORE']");

    private By OnlinePlayersElement => By.XPath(
        "//*[contains(@class,'online_stat_label') and contains(@class,'gamers_online')]" +
        "/parent::*[contains(@class,'online_stat')]");

    private By ActivePlayersElement => By.XPath(
        "//*[contains(@class,'online_stat_label') and contains(@class,'gamers_in_game')]" +
        "/parent::*[contains(@class,'online_stat')]");

    public bool IsAt()
    {
        try
        {
            return Find(AboutPageIndicator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public StorePage GoToStorePage(IWebDriver driver)
    {
        Find(StoreButton).Click();
        return new StorePage(driver);
    }

    public long GetActivePlayers()
    {
        try
        {
            var text = Find(ActivePlayersElement).Text;
            Console.WriteLine($"Активные пользователи с сайта:{text}");
            var onlyNumber = Regex.Replace(text, @"[^\d]", "");
            Console.WriteLine($"Только цифры:{onlyNumber}");
            var number = long.Parse(onlyNumber);
            Console.WriteLine($"Число в long:{number}");
            return number;
        }
        catch
        {
            return 0;
        }
    }

    public long GetOnlinePlayers()
    {
        try
        {
            var text = Find(OnlinePlayersElement).Text;
            var onlyNumber = Regex.Replace(text, @"[^\d]", "");
            var number = long.Parse(onlyNumber);
            Console.WriteLine($"Online players:{number}");
            return number;
        }
        catch
        {
            return 0;
        }
    }
}