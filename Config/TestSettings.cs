namespace ShopAutomation.Config;

/// <summary>Plain options object bound from configuration. Created only by <see cref="SettingsProvider"/>.</summary>
public class TestSettings : ITestSettings
{
    public string BaseUrl { get; set; }
    public string Browser { get; set; } = "chrome";
    public bool Headless { get; set; }
    public int DefaultTimeoutSeconds { get; set; } = 15;
    public string ScreenshotDir { get; set; } = "Screenshots";
}
