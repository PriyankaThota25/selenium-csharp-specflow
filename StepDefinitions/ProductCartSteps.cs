using FluentAssertions;
using ShopAutomation.Pages;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ShopAutomation.StepDefinitions;

[Binding]
public class ProductCartSteps
{
    private readonly ProductsPage _products;
    private readonly CartPage _cart;

    public ProductCartSteps(ProductsPage products, CartPage cart)
    {
        _products = products;
        _cart = cart;
    }

    [Given(@"I am on the products page")]
    public void GivenIAmOnTheProductsPage() => _products.GoTo();

    [When(@"I search for ""(.*)""")]
    public void WhenISearchFor(string term) => _products.SearchFor(term);

    [Then(@"the search results should include ""(.*)""")]
    public void ThenResultsInclude(string name) =>
        _products.ResultNames().Should().Contain(n => n.Equals(name, StringComparison.OrdinalIgnoreCase));

    [When(@"I add ""(.*)"" to the cart")]
    public void WhenIAddToCart(string name) => _products.AddProductToCart(name);

    [When(@"I open the cart")]
    public void WhenIOpenTheCart() => _products.ViewCart();

    [Then(@"the cart should contain the following items")]
    public void ThenCartShouldContain(Table table)
    {
        var expected = table.Rows.Select(r => (Name: r["Product"], Qty: int.Parse(r["Quantity"]))).ToList();
        var actual = _cart.Lines().Select(l => (l.Name, Qty: l.Quantity)).ToList();
        actual.Should().BeEquivalentTo(expected);
    }
}
