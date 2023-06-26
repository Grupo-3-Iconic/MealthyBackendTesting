Feature: RecipeServiceTest
  As a Developer
  I want to add a new Recipe Through the API
  In order to make it available to the users.
	
    Background: 
        Given the endpoint https://localhost:44327/api/v1/recipe is available
    @recipe-adding
    Scenario: Add recipe with unique title
      When a Post request is sent
        | title            | description            | preparationTime | servings |
        | Pollo a la Brasa | Pollo marinado y asado | 2 horas         | 4        |
      Then A Response is returned with Status 200
      And A Recipe Resource is included in the response body
        | id | title            | description            | preparationTime | servings | ingredients | steps |
        | 1  | Pollo a la Brasa | Pollo marinado y asado | 2 horas         | 4        |             |       |

    Scenario: Add recipe with existing title
      Given a recipe is already stored
        | id | title               | description                | preparationTime | servings | ingredients | steps |
        | 1  | Spaghetti Carbonara | Classic Italian pasta dish | 30 minutes      | 2        |             |       |
      When a Post request is sent
        | title               | description                    | preparationTime | servings |
        | Spaghetti Carbonara | Authentic Italian pasta recipe | 45 minutes      | 4        |
      Then A Response is returned with Status 400
      And An Error is returned with value "Ya existe una receta con el mismo nombre."
    
    Scenario: Add recipe with missing data
      When a Post request is sent
        | title | description | preparationTime | servings |
        |       |             |                 |          |
      Then A Response is returned with Status 400