using FluentAssertions;
using ShopAutomation.Pages;
using TechTalk.SpecFlow;

namespace ShopAutomation.StepDefinitions;

[Binding]
public class CartManagementSteps
{
    private readonly CartPage _cart;

    public CartManagementSteps(CartPage cart) => _cart = cart;

    [When(@"I remove ""(.*)"" from the cart")]
    public void WhenIRemoveFromTheCart(string name) => _cart.RemoveItem(name);

    [Then(@"the cart should be empty")]
    public void ThenTheCartShouldBeEmpty() => _cart.IsEmpty().Should().BeTrue();
}
