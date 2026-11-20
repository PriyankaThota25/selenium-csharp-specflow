@ui @details
Feature: Product details
  As a shopper on Automation Exercise
  I want to open a product's detail page
  So that I can see its price, category and availability

  @smoke
  Scenario: View the details of a product from the product list
    Given I am on the products page
    When I view the details of "Blue Top"
    Then the product details should be
      | Name     | Category     | Price   | Availability | Condition | Brand |
      | Blue Top | Women > Tops | Rs. 500 | In Stock     | New       | Polo  |
