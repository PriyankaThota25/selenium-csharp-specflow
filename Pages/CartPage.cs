using OpenQA.Selenium;
using ShopAutomation.Config;

namespace ShopAutomation.Pages;

public record CartLine(string Name, string Price, int Quantity, string Total);

public class CartPage : BasePage
{
    private static readonly By Rows = By.CssSelector("#cart_info_table tbody tr");

    public CartPage(IWebDriver driver, ITestSettings settings) : base(driver, settings) { }

    private static readonly By EmptyCartMessage = By.Id("empty_cart");

    public bool IsEmpty()
    {
        Wait.Until(d => d.FindElements(Rows).Count == 0);
        return WaitVisible(EmptyCartMessage).Displayed;
    }

    public CartPage RemoveItem(string productName)
    {
        WaitAll(Rows);
        var row = Driver.FindElements(Rows).First(r =>
            r.FindElement(By.CssSelector(".cart_description h4 a")).GetAttribute("textContent").Trim()
             .Equals(productName, StringComparison.OrdinalIgnoreCase));
        JsClick(row.FindElement(By.CssSelector(".cart_quantity_delete")));
        return this;
    }

    public IReadOnlyList<CartLine> Lines()
    {
        WaitAll(Rows);
        return Driver.FindElements(Rows).Select(r => new CartLine(
            Name: r.FindElement(By.CssSelector(".cart_description h4 a")).GetAttribute("textContent").Trim(),
            Price: r.FindElement(By.CssSelector(".cart_price p")).GetAttribute("textContent").Trim(),
            Quantity: int.Parse(r.FindElement(By.CssSelector(".cart_quantity button")).GetAttribute("textContent").Trim()),
            Total: r.FindElement(By.CssSelector(".cart_total_price")).GetAttribute("textContent").Trim()
        )).ToList();
    }
}
