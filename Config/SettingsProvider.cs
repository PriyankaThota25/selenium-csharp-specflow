using Microsoft.Extensions.Configuration;

namespace ShopAutomation.Config;

/// <summary>
/// Classic eager Singleton: sealed (no subclassing), private constructor (no outside instantiation),
/// single static readonly instance created by the CLR when the type is first initialised
/// (the CLR guarantees this is thread-safe).
/// Loads appsettings.json; env vars prefixed TEST_ override (e.g. TEST_HEADLESS=true).
/// </summary>
public sealed class SettingsProvider : ITestSettings
{
    public static readonly SettingsProvider Instance = new();

    private readonly TestSettings _settings;

    // Private: the only instance is Instance above.
    private SettingsProvider()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables("TEST_")
            .Build();
        _settings = config.Get<TestSettings>();
    }

    public string BaseUrl => _settings.BaseUrl;
    public string Browser => _settings.Browser;
    public bool Headless => _settings.Headless;
    public int DefaultTimeoutSeconds => _settings.DefaultTimeoutSeconds;
    public string ScreenshotDir => _settings.ScreenshotDir;
}
