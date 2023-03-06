# Designing and Developing a Clean REST Web API Architecture

This project aims to develop a clean and efficient RESTful Web API using the following technologies:

- Web API: .NET Core 6 Web API
- Database Management: Entity Framework Core 6
- Identity: .NET Core Identity Role-Based
- Authentication/Authorization: Identity is configured to use JWT Tokens for authentication

## Solution Architecture

The solution is organized into the following directories:

- `src`
  - `BreweryWholesaleService.Api`
  - `BreweryWholesaleService.Core`
  - `BreweryWholesaleService.Infrastructure`
- `Tests`
  - `BreweryWholesaleService.Api.Tests`
  - `BreweryWholesaleService.Core.Tests`
  - `BreweryWholesaleService.Shared.Tests`

## Database Design

In this application, breweries and wholesalers are represented as users with Brewery and Wholesaler roles. The Web API is supported with a Swagger UI, where the API functionality is fully tested using the Swagger interface.

To test controller actions that require authorization, follow these steps:

1. Get the user JWT token by using the `api/UserManager/getToken` API and providing the username and password.
2. Enter the authorization token via the Swagger UI.

## Testing

We have implemented testing using .NET Core 6 xUnit for API-level testing and core-level testing using mocking, auto fixture, and fluent assertion.

API-level testing includes:

- BeerControllerTest
  - `GetBeersByBrewery`: Should return an OK response when data is found.
  - `GetBeersByBrewery`: Should return a NotFound response when the brewery username is invalid.
  - `AddNewBeer`: Should return an OK response when passing a valid model.

Core-level testing includes:

- SaleServiceTest
  - `GetSaleQuote`: Should return data when a valid quote request satisfies stock quantities.
  - `GetSaleQuote`: Should throw an exception when the number of beers ordered exceeds the wholesaler's stock.
  - `GetSaleQuote`: Should throw an exception when the quote request contains beers that are not sold by the wholesaler.
  - `GetSaleQuote`: Should throw an exception when the quote request contains duplicate beer orders.

Overall, this project demonstrates best practices for developing a clean and efficient RESTful Web API using .NET Core 6 and related technologies.
