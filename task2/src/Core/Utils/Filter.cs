namespace task2.Core.Utils;

public class Filter
{
    public string Block { get; set; } = "";
    public string Name { get; set; } = "";
    public bool UseSearch { get; set; } = false;
    public string? SearchText { get; set; }
}