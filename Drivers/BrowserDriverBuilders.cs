using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using ShopAutomation.Config;

namespace ShopAutomation.Drivers;

// Selenium Manager (bundled with Selenium 4.6+) downloads the matching driver automatically.

public class ChromeDriverBuilder : IBrowserDriverBuilder
{
    public string BrowserName => "chrome";

    public IWebDriver Build(ITestSettings s)
    {
        var o = new ChromeOptions();
        if (s.Headless) o.AddArgument("--headless=new");
        o.AddArguments("--window-size=1920,1080", "--disable-notifications",
            "--disable-popup-blocking", "--disable-search-engine-choice-screen");
        // CI Linux containers/agents: no usable sandbox and a tiny /dev/shm make Chrome crash on start.
        if (OperatingSystem.IsLinux()) o.AddArguments("--no-sandbox", "--disable-dev-shm-usage");
        return new ChromeDriver(o);
    }
}

public class FirefoxDriverBuilder : IBrowserDriverBuilder
{
    public string BrowserName => "firefox";

    public IWebDriver Build(ITestSettings s)
    {
        var o = new FirefoxOptions();
        if (s.Headless) o.AddArgument("-headless");
        return new FirefoxDriver(o);
    }
}

public class EdgeDriverBuilder : IBrowserDriverBuilder
{
    public string BrowserName => "edge";

    public IWebDriver Build(ITestSettings s)
    {
        var o = new EdgeOptions();
        if (s.Headless) o.AddArgument("--headless=new");
        return new EdgeDriver(o);
    }
}
