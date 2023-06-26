Feature: StepServiceTest
	As a Developer
	I want to add a steps for a recipe Through the API
	In order to make it available to the users.
	Background: 
		Given the endpoint https://localhost:44327/api/v1/step is available
	@step-adding
	Scenario: Add step with complete data
		When a Post step request is sent
		  | description | recipeId |
		  | Lava el pollo c: |   1  |
		Then the post step response should be status 200
		And the post step response body should be
		  | id | description      | recipeId |
		  |  1 | Lava el pollo c: | 1        |
    Scenario: Add step with missing data
    	When a Post step request is sent
    	| description | recipeId |
        |             |          |
		Then the post step response should be status 400