namespace task2.Core.Utils;

public class TestSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public bool Incognito { get; set; } = true;
    public bool Headless { get; set; } = false;
    public int ImplicitWaitSec { get; set; } = 5;
    public string UiLanguage { get; set; } = "en";
    public bool Maximize { get; set; } = true;
    
    
}