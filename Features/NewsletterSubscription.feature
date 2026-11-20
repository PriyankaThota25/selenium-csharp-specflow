@ui @subscription
Feature: Newsletter subscription
  As a visitor to Automation Exercise
  I want to subscribe to the newsletter from the footer
  So that I receive news and offers

  @smoke
  Scenario: Subscribe to the newsletter from the home page footer
    Given I am on the home page
    When I subscribe to the newsletter with "qa.tester@example.com"
    Then I should see the subscription message "You have been successfully subscribed!"
