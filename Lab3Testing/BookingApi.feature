Feature: Booking API

  Scenario: Create a new booking
    Given I am authenticated
    When I create a new booking with the following details
      | firstname | lastname | totalprice | depositpaid | checkin     | checkout    | additionalneeds |
      | Daria     | Mazur    | 13         | true        | 2024-11-19  | 2024-11-20  | Nothing         |
    Then the booking should be created successfully
