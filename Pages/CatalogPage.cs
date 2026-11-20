using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

/// <summary>Category / brand browsing via the left sidebar of the products pages.</summary>
public class CatalogPage : BasePage
{
    private static readonly By Title = By.CssSelector(".features_items .title");
    private static readonly By ProductNamesLocator = By.CssSelector(".features_items .productinfo p");

    public CatalogPage(IWebDriver driver, ITestSettings settings) : base(driver, settings) { }

    public CatalogPage GoTo()
    {
        Open("/products");
        WaitVisible(Title);
        return this;
    }

    public CatalogPage OpenCategory(string parent, string sub)
    {
        // Sidebar is a collapsed accordion, so click the (hidden) link directly.
        var link = WaitPresent(By.XPath($"//div[@id='{parent}']//a[normalize-space()='{sub}']"));
        JsClick(link);
        DismissInterruptions();
        return this;
    }

    public CatalogPage OpenBrand(string brand)
    {
        var link = WaitPresent(By.XPath($"//div[@class='brands-name']//a[@href='/brand_products/{brand}']"));
        JsClick(link);
        DismissInterruptions();
        return this;
    }

    public string HeadingText()
    {
        Wait.Until(d => d.FindElement(Title).Text.Length > 0);
        return System.Text.RegularExpressions.Regex.Replace(Driver.FindElement(Title).Text, @"\s+", " ").Trim();
    }

    public IReadOnlyList<string> ProductNames() =>
        WaitAll(ProductNamesLocator).Select(e => e.GetAttribute("textContent").Trim()).ToList();
}
