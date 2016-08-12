Feature: The Social Network commandline client should be interactive

    Scenario: waiting for user input after launch
		When I launch the client
		And I wait for 1 second
		Then the client should still be there
