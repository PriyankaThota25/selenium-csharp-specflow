@ui @catalog
Feature: Brand filter
  As a shopper on Automation Exercise
  I want to filter products by brand
  So that I only see items from the brand I trust

  Scenario Outline: Filter products by brand
    Given I am on the catalog page
    When I filter products by the brand "<Brand>"
    Then the results heading should be "Brand - <Brand> Products"
    And at least one product should be listed

    Examples:
      | Brand |
      | Polo  |
      | Biba  |
