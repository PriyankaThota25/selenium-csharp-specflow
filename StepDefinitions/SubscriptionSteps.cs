using FluentAssertions;
using ShopAutomation.Pages;
using TechTalk.SpecFlow;

namespace ShopAutomation.StepDefinitions;

[Binding]
public class SubscriptionSteps
{
    private readonly HomePage _home;

    public SubscriptionSteps(HomePage home) => _home = home;

    [Given(@"I am on the home page")]
    public void GivenIAmOnTheHomePage() => _home.GoTo();

    [When(@"I subscribe to the newsletter with ""(.*)""")]
    public void WhenISubscribeWith(string email) => _home.SubscribeWith(email);

    [Then(@"I should see the subscription message ""(.*)""")]
    public void ThenISeeTheSubscriptionMessage(string message) =>
        _home.SubscriptionMessage().Should().Be(message);
}
