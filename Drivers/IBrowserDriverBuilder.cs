using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Drivers;

/// <summary>Strategy for building one browser's driver. Add a new browser by adding a class - no factory edits (OCP).</summary>
public interface IBrowserDriverBuilder
{
    string BrowserName { get; }
    IWebDriver Build(ITestSettings settings);
}
