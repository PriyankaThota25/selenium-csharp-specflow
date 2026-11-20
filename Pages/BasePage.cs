using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    protected readonly ITestSettings Settings;

    protected BasePage(IWebDriver driver, ITestSettings settings)
    {
        Driver = driver;
        Settings = settings;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(settings.DefaultTimeoutSeconds));
        Wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    protected IWebElement WaitVisible(By by) =>
        Wait.Until(d => { var e = d.FindElement(by); return e.Displayed ? e : null; });

    protected IReadOnlyCollection<IWebElement> WaitAll(By by) =>
        Wait.Until(d => { var l = d.FindElements(by); return l.Count > 0 ? l : null; });

    /// <summary>Waits for an element to exist in the DOM (even if hidden, e.g. collapsed accordion links).</summary>
    protected IWebElement WaitPresent(By by) => Wait.Until(d => d.FindElement(by));

    // "Added to cart" modal is shown after adding a product from any page.
    private static readonly By CartModal = By.Id("cartModal");
    private static readonly By ViewCartLink = By.CssSelector("#cartModal a[href='/view_cart']");

    protected void WaitForCartModal() => WaitVisible(CartModal);

    protected void ClickViewCartInModal() => JsClick(WaitVisible(ViewCartLink));

    protected void Type(By by, string text)
    {
        var e = WaitVisible(by);
        e.Clear();
        e.SendKeys(text);
    }

    /// <summary>JS click: robust against the ad overlays and hover-only buttons this site uses.</summary>
    protected void JsClick(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
    }

    protected void Open(string path)
    {
        Driver.Navigate().GoToUrl(Settings.BaseUrl.TrimEnd('/') + path);
        DismissInterruptions();
    }

    /// <summary>Closes the cookie-consent dialog and Google ad overlays if present.</summary>
    protected void DismissInterruptions()
    {
        try
        {
            var consent = Driver.FindElements(By.CssSelector("button.fc-cta-consent"));
            if (consent.Count > 0 && consent[0].Displayed) consent[0].Click();
        }
        catch (WebDriverException) { /* best effort */ }

        // Ad vignette redirects add #google_vignette to the URL; strip it.
        if (Driver.Url.Contains("#google_vignette"))
            Driver.Navigate().GoToUrl(Driver.Url.Replace("#google_vignette", ""));
    }
}
