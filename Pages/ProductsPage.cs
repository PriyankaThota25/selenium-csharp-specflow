using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

public class ProductsPage : BasePage
{
    private static readonly By SearchInput = By.Id("search_product");
    private static readonly By SearchButton = By.Id("submit_search");
    private static readonly By ResultsTitle = By.CssSelector(".features_items .title");
    private static readonly By ProductCards = By.CssSelector(".features_items .product-image-wrapper");

    public ProductsPage(IWebDriver driver, ITestSettings settings) : base(driver, settings) { }

    public ProductsPage GoTo()
    {
        Open("/products");
        WaitVisible(SearchInput);
        return this;
    }

    public ProductsPage SearchFor(string term)
    {
        Type(SearchInput, term);
        JsClick(WaitVisible(SearchButton));
        Wait.Until(d => d.FindElement(ResultsTitle).Text.Contains("SEARCHED PRODUCTS", StringComparison.OrdinalIgnoreCase));
        return this;
    }

    public IReadOnlyList<string> ResultNames() =>
        WaitAll(ProductCards)
            .Select(c => c.FindElement(By.CssSelector(".productinfo p")).GetAttribute("textContent").Trim())
            .ToList();

    public ProductsPage AddProductToCart(string productName)
    {
        var card = WaitAll(ProductCards).First(c =>
            c.FindElement(By.CssSelector(".productinfo p")).GetAttribute("textContent").Trim()
             .Equals(productName, StringComparison.OrdinalIgnoreCase));
        // The visible "Add to cart" is in a hover overlay; the one inside .productinfo is always in the DOM.
        JsClick(card.FindElement(By.CssSelector(".productinfo a.add-to-cart")));
        WaitForCartModal();
        return this;
    }

    public ProductsPage OpenProductDetails(string productName)
    {
        var card = WaitAll(ProductCards).First(c =>
            c.FindElement(By.CssSelector(".productinfo p")).GetAttribute("textContent").Trim()
             .Equals(productName, StringComparison.OrdinalIgnoreCase));
        JsClick(card.FindElement(By.CssSelector(".choose a[href^='/product_details']")));
        DismissInterruptions();
        return this;
    }

    /// <summary>Clicks "View Cart"; use the injected <see cref="CartPage"/> to inspect it.</summary>
    public ProductsPage ViewCart()
    {
        ClickViewCartInModal();
        return this;
    }
}
