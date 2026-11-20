using OpenQA.Selenium;

namespace ShopAutomation.Drivers;

public interface IDriverFactory
{
    IWebDriver Create();
}
