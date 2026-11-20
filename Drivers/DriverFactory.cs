using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Drivers;

public class DriverFactory : IDriverFactory
{
    private readonly ITestSettings _settings;
    private readonly IReadOnlyDictionary<string, IBrowserDriverBuilder> _builders;

    public DriverFactory(ITestSettings settings, IEnumerable<IBrowserDriverBuilder> builders)
    {
        _settings = settings;
        _builders = builders.ToDictionary(b => b.BrowserName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Default wiring used by the composition root.</summary>
    public static DriverFactory CreateDefault(ITestSettings settings) => new(settings, new IBrowserDriverBuilder[]
    {
        new ChromeDriverBuilder(), new FirefoxDriverBuilder(), new EdgeDriverBuilder()
    });

    public IWebDriver Create()
    {
        // Unknown browser names fall back to Chrome (previous behaviour).
        var builder = _builders.TryGetValue(_settings.Browser ?? "chrome", out var b) ? b : _builders["chrome"];
        var driver = builder.Build(_settings);
        driver.Manage().Window.Maximize();
        // Explicit waits only - no implicit wait, to avoid mixing the two.
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        return driver;
    }
}
