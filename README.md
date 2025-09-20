# Table of Contents

- [Summary](#summary)
- [Projects](#projects)
- [Start Developing](#start-developing)
  - [Backend](#backend)
    - [Run projects](#run-projects)
    - [Logging](#logging)
    - [Run tests](#run-tests)
    - [Linting and formatting](#linting-and-formatting)
  - [Frontend](#frontend)

# Summary

This repository contains all projects related to the Tikal project. An attempt to implement the classic
game [Tikal](<https://en.wikipedia.org/wiki/Tikal_(board_game)>) as a browser game.

The game can be found under <a>https://tikalonline.com</a> (WIP).

# Projects

The repository contains the following projects:

- **Aspire:** The Aspire project used to orchestrate the development environment.
- **Frontend:** The angular frontend, includes tests
- **IdentityAPI:** The api exclusively responsible for authentication of users. It provides jwt tokens which are used
  for authorization in the other parts of the system.
- **IdentityAPI.Tests:** Contains unit tests for the IdentityAPI.
- **IdentityAPI.Integration:** Contains integration tests for the IdentityAPI.
- **Infrastructure:** Documentation and configuration of the infrastructure on which the system is deployed.
- **Tikal:** Contains all projects related to the main backend for the Tikal game.
- **Tikal.Tests:** Contains all test projects related to the main backend for the Tikal game.

For more details refer to the READMEs in the respective sub folders.

# Start Developing

## Run Projects

This project is developed using [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview). It is not recommended to try to run the individual projects directly.

## Backend

### Run Tests

To run the tests of a specific project run the following command:

```
dotnet test .\{project_name}\{project_name}.csproj
```

If you want to run all tests in the solution run:

```
dotnet test .\Tikal.sln
```

The integration tests make use of test containers. So make sure to have a docker environment available when executing
integration tests.

### Linting and formatting

To apply the linting and formatting rules to a specific project run the following command:

<pre>
dotnet format .\{project_name}\{project_name}.csproj
</pre>

The rules are defined using .editorconfig in the root directory.

## Frontend

For everything frontend related please refer to the README.md in ./Frontend
