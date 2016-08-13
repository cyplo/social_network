Feature: Posting messages

    Anyone at the terminal should be able to post a message to their timeline.

    Scenario: Posting first message
        When I type "Alice -> I love the weather today"
        Then the prompt should still be there

    Scenario: Two users posting one after another
        When I type "Alice -> I love the weather today"
        And I pass the keyboard to another person
        And they type "Bob -> Damn! We lost!"
        Then the prompt should still be there

    Scenario: Posting subsequent messages
        Given I've already posted as Alice
        When I type "Alice -> I love the weather today"
        Then the prompt should still be there
