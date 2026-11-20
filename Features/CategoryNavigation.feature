@ui @catalog
Feature: Category navigation
  As a shopper on Automation Exercise
  I want to browse products by category
  So that I can find items in the section I care about

  Scenario Outline: Browse a product category from the sidebar
    Given I am on the catalog page
    When I browse the "<Section>" category "<Category>"
    Then the results heading should be "<Heading>"
    And at least one product should be listed

    Examples:
      | Section | Category | Heading               |
      | Women   | Dress    | Women - Dress Products |
      | Men     | Tshirts  | Men - Tshirts Products |
      | Kids    | Dress    | Kids - Dress Products  |
