using FluentAssertions;
using ShopAutomation.Pages;
using TechTalk.SpecFlow;

namespace ShopAutomation.StepDefinitions;

[Binding]
public class ProductDetailsSteps
{
    private readonly ProductsPage _products;
    private readonly ProductDetailsPage _details;

    public ProductDetailsSteps(ProductsPage products, ProductDetailsPage details)
    {
        _products = products;
        _details = details;
    }

    [When(@"I view the details of ""(.*)""")]
    public void WhenIViewTheDetailsOf(string name) => _products.OpenProductDetails(name);

    [Then(@"the product details should be")]
    public void ThenTheProductDetailsShouldBe(Table table)
    {
        var row = table.Rows[0];
        var expected = new ProductDetails(row["Name"], row["Category"], row["Price"],
            row["Availability"], row["Condition"], row["Brand"]);
        _details.Details().Should().Be(expected);
    }

    [When(@"I set the quantity to (\d+)")]
    public void WhenISetTheQuantityTo(int quantity) => _details.SetQuantity(quantity);

    [When(@"I add the product to the cart from its details page")]
    public void WhenIAddTheProductToTheCartFromDetails() => _details.AddToCart();

    [When(@"I open the cart from the details page")]
    public void WhenIOpenTheCartFromDetails() => _details.ViewCart();
}
