Feature: Viewing user's timeline

    Anyone should be able to view any user's timeline of messages.
	Timeline is an aggregate of all messages posted by the user, sorted by time of posting.

    Scenario: Reading the only message posted
        When I type "Alice -> I love the weather today"
		And I type "Alice"
		Then I should see "I love the weather today (just now)"