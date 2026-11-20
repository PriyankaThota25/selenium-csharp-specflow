using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

public class HomePage : BasePage
{
    private static readonly By SubscribeEmail = By.Id("susbscribe_email"); // sic - id is misspelled on the site
    private static readonly By SubscribeButton = By.Id("subscribe");
    private static readonly By SubscribeSuccess = By.CssSelector("#success-subscribe .alert-success");

    public HomePage(IWebDriver driver, ITestSettings settings) : base(driver, settings) { }

    public HomePage GoTo()
    {
        Open("/");
        return this;
    }

    public HomePage SubscribeWith(string email)
    {
        var input = WaitPresent(SubscribeEmail);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
        Type(SubscribeEmail, email);
        JsClick(WaitPresent(SubscribeButton));
        return this;
    }

    public string SubscriptionMessage() => WaitVisible(SubscribeSuccess).Text.Trim();
}
