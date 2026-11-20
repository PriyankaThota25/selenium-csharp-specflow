@ui @cart
Feature: Cart management
  As a shopper on Automation Exercise
  I want to control what is in my cart
  So that I only buy what I intend to

  Scenario: Add several units of a product from its details page
    Given I am on the products page
    When I view the details of "Blue Top"
    And I set the quantity to 3
    And I add the product to the cart from its details page
    And I open the cart from the details page
    Then the cart should contain the following items
      | Product  | Quantity |
      | Blue Top | 3        |

  Scenario: Remove the only item from the cart
    Given I am on the products page
    When I add "Blue Top" to the cart
    And I open the cart
    And I remove "Blue Top" from the cart
    Then the cart should be empty
