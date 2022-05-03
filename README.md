# Goal-Inventory

## Build and run

Open a CLI, navigate to the root of the solution and type:

```
dotnet run --project Inventory.Api
```

Or open VisualStudio or Rider, set Inventory.Api as the startup project, and run.

I have also implemented a MinimalApi project that runs with:

```
dotnet run --project Inventory.MinimalApi
```

Or with VisualStudio or Rider, set Inventory.MinimalApi as the startup project, and run.

I'm used MinimalApi because it's the simplest solution, I never used it before but I'm sure it's a good start.
Surely for a real project I would use classic WebApi implementation.

## Patterns used
   - Dependency injection (DI) using .Net Core framework
   - Sub/Pub pattern for event-driven architecture
   - CQRS pattern for command/query separation
   - Repository pattern for data access

### Note
CQRS implementation is a POC made by me for fun and it can be found in https://github.com/soylegendario/Cqrs

## Third party libraries are used to make the project work:
   - Moq: To mock object in unit tests
   - FluentAssertions: To assert the result of the tests
   - FluentValidation: To validate input data
   - Swashbuckle.AspNetCore.Annotations: To generate swagger documentation

## Third party libraries would use in the real life (in addition to the above)
   - Entity Framework Core: To store data in a database
   - Dapper: To access the database
   - Automapper: To map data between objects
   - RabbitMQ / Azure Queue: To send messages and distributed queue
   - Azure Functions: To schedule tasks (notify expired items)