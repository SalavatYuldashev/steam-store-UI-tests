using Microsoft.Extensions.Configuration;

namespace task2.Core.Utils;

public static class FiltersReader
{
    public static FilterConfig Load()
    {
        var root = new ConfigurationBuilder()
            .AddJsonFile("filters.json", optional: false, reloadOnChange: true)
            .Build();

        var cfg = new FilterConfig();
        root.Bind(cfg);
        return cfg;
    }
}