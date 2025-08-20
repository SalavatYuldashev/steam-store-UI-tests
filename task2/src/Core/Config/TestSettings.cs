namespace task2.Core.Config;

public class TestSettings
{
    public static string BaseUrl { get; set; } = string.Empty;
    public static bool Incognito { get; set; } = true;
    public static bool Headless { get; set; } = false;
    public static int ImplicitWaitSec { get; set; } = 5;
    public static string UILanguage { get; set; } = "en";
    public static bool Maximize { get; set; } = true;
    
}