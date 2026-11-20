@ui @cart
Feature: Product search and shopping cart
  As a shopper on Automation Exercise
  I want to search for a product and add it to my cart
  So that I can review it before checkout

  @smoke
  Scenario: Search for a product, add it to the cart and verify the cart contents
    Given I am on the products page
    When I search for "Blue Top"
    Then the search results should include "Blue Top"
    When I add "Blue Top" to the cart
    And I open the cart
    Then the cart should contain the following items
      | Product  | Quantity |
      | Blue Top | 1        |
