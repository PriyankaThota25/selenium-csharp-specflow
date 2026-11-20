using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

public record ProductDetails(string Name, string Category, string Price, string Availability, string Condition, string Brand);

public class ProductDetailsPage : BasePage
{
    private static readonly By Info = By.CssSelector(".product-information");
    private static readonly By NameHeading = By.CssSelector(".product-information h2");
    private static readonly By Category = By.XPath("//div[@class='product-information']/p[starts-with(normalize-space(.),'Category:')]");
    private static readonly By Price = By.CssSelector(".product-information > span > span");
    private static readonly By QuantityInput = By.Id("quantity");
    private static readonly By AddToCartButton = By.CssSelector(".product-information button.cart");

    public ProductDetailsPage(IWebDriver driver, ITestSettings settings) : base(driver, settings) { }

    public ProductDetails Details()
    {
        WaitVisible(Info);
        return new ProductDetails(
            Name: Text(NameHeading),
            Category: Text(Category).Replace("Category:", "").Trim(),
            Price: Text(Price),
            Availability: Field("Availability"),
            Condition: Field("Condition"),
            Brand: Field("Brand"));
    }

    public ProductDetailsPage SetQuantity(int quantity)
    {
        Type(QuantityInput, quantity.ToString());
        return this;
    }

    public ProductDetailsPage AddToCart()
    {
        JsClick(WaitVisible(AddToCartButton));
        WaitForCartModal();
        return this;
    }

    public void ViewCart() => ClickViewCartInModal();

    private string Text(By by) => WaitVisible(by).GetAttribute("textContent").Trim();

    // Rows look like: <p><b>Brand:</b> Polo</p>
    private string Field(string label) =>
        Text(By.XPath($"//div[@class='product-information']/p[b[normalize-space()='{label}:']]"))
            .Substring(label.Length + 1).Trim();
}
