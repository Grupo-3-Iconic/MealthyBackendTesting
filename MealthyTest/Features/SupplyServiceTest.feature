Feature: SupplyServiceTest
	As a Developer
	I want to add a supplies Through the API
	In order fill to pantry
	Background: 
		Given the endpoint https://localhost:44327/api/v1/supply is available
	@supply-adding
		Scenario: Add supply with complete data
			When a post supply request is sent
			| name | quantity | unit |
			| Milk | 1 | Litre |
			Then the post supply response should be status 200
			And the post supply response body should be
			| id | name | quantity | unit  |
			| 1  | Milk | 1        | Litre |   
   		Scenario: Add supply with missing data
   			When a post supply request is sent
   			| name | quantity | unit |
            | Milk | 1        |      |
            Then the post supply response should be status 400