Feature: MarketServiceTest
	As a Developer
	I want to add a markets Through the API
	In order fill the database with markets
	
	Background: 
		Given the endpoint https://localhost:44327/api/v1/market is available
	@market-adding
	Scenario: Add market with complete data
		When a post market request is sent
		| storeName | description | firstName | lastName | ruc | email | password | location | phone | photo |
		| storeName | description | firstName | lastName | ruc | email | password | location | phone | photo |
		Then the post market response should be status 201
		And the post market response body should be
		  | id | storeName | description | firstName | lastName | ruc | email | password | location | phone | photo |
		  |  1 | storeName | description | firstName | lastName | ruc | email | password | location | phone | photo |
    Scenario: Add market with missing data
    	When a post market request is sent
    	| storeName | description | firstName | lastName | ruc | email | password | location | phone | photo |
        | Tienda    |             |           |          | ruc | email | password | location | phone | photo |
        Then the post market response should be status 400