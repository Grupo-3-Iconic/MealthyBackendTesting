Feature: IngredientServiceTest
	As a Developer
	I want to add a ingredient for a recipe Through the API
	In order to make it available to the users.
	Background: 
		Given the endpoint https://localhost:44327/api/v1/ingredient is available
	@ingredient-adding
	Scenario: Add ingredient with complete data
		When a Post ingredient request is sent
		| name | quantity | unit | recipeId |
		| Pollo | 1 | Kilogramo | 1 |
		Then the ingredient response should be status 200
		And the ingredient response body should be
		 | id | name  | quantity | unit      | recipeId |
		 | 1  | Pollo | 1        | Kilogramo | 1        |
	Scenario: Add ingredient with missing data
		When a Post ingredient request is sent
		  | name  | quantity | unit      | recipeId |
		  |  |      1    | Kilogramo | 1        |
		Then the response should be status 400
