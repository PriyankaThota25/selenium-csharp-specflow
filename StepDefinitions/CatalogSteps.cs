using FluentAssertions;
using ShopAutomation.Pages;
using TechTalk.SpecFlow;

namespace ShopAutomation.StepDefinitions;

[Binding]
public class CatalogSteps
{
    private readonly CatalogPage _catalog;

    public CatalogSteps(CatalogPage catalog) => _catalog = catalog;

    [Given(@"I am on the catalog page")]
    public void GivenIAmOnTheCatalogPage() => _catalog.GoTo();

    [When(@"I browse the ""(.*)"" category ""(.*)""")]
    public void WhenIBrowseTheCategory(string parent, string sub) => _catalog.OpenCategory(parent, sub);

    [When(@"I filter products by the brand ""(.*)""")]
    public void WhenIFilterByBrand(string brand) => _catalog.OpenBrand(brand);

    [Then(@"the results heading should be ""(.*)""")]
    public void ThenTheResultsHeadingShouldBe(string heading) =>
        _catalog.HeadingText().Should().BeEquivalentTo(heading);

    [Then(@"at least one product should be listed")]
    public void ThenAtLeastOneProductShouldBeListed() =>
        _catalog.ProductNames().Should().NotBeEmpty().And.OnlyContain(n => !string.IsNullOrWhiteSpace(n));
}
