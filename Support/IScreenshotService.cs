using OpenQA.Selenium;

namespace ShopAutomation.Support;

public interface IScreenshotService
{
    /// <summary>Saves a screenshot to <paramref name="path"/>; returns false if the driver can't take screenshots.</summary>
    bool TryCapture(IWebDriver driver, string path);
}

public class ScreenshotService : IScreenshotService
{
    public bool TryCapture(IWebDriver driver, string path)
    {
        if (driver is not ITakesScreenshot shot) return false;
        shot.GetScreenshot().SaveAsFile(path);
        return true;
    }
}
