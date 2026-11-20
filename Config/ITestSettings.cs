namespace ShopAutomation.Config;

/// <summary>Read-only view of test configuration. Consumers depend on this, not on the concrete class (DIP/ISP).</summary>
public interface ITestSettings
{
    string BaseUrl { get; }
    string Browser { get; }
    bool Headless { get; }
    int DefaultTimeoutSeconds { get; }
    string ScreenshotDir { get; }
}
