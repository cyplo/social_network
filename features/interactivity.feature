Feature: The Social Network commandline client should be interactive

    Scenario: waiting for user input after launch
		When I wait for 1 second
		Then the prompt should still be there
