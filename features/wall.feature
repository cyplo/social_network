Feature: The Wall

  Anyone should be able to follow any other user to see their messages.

  Scenario: No one has any messages, one user follows another
    When I type "Charlie follows Alice"
    Then the prompt should still be there

  Scenario: No one has any messages,
            one user follows another and tries to see their wall
    When Charlie follows Alice
    And I type "Charlie wall"
    Then the prompt should still be there

  Scenario: Seeing past message on the wall
    When Charlie posts "I'm in New York today! Anyone want to have a coffee?"
    And I type "Charlie wall"
    Then I should see "Charlie - I'm in New York today! Anyone want to have a coffee?"

  Scenario: Seeing messages posted after user followed another
    Given Charlie follows Alice
    When Alice posts "I love the weather today"
    And I type "Charlie wall"
    Then I should see "Alice - I love the weather today"