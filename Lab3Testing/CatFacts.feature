Feature: Cat Facts API

  Scenario: Request a random cat fact
    When I request a random cat fact
    Then the status code should be 200
    And the response should contain a non-empty 'fact' field
