Feature: Exiting the Social Network commandline client

	Scenario: exiting right after launch
		When I launch the client
		And I type "exit"
		Then the client should quit 
