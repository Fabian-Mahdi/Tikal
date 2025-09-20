# Table of Contents

- [Summary](#summary)
- [Projects](#projects)
- [Run Tests](#run-tests)

# Summary

This directory contains all tests for the main backend of the Tikal game.

# Projects

It contains the following projects:

- **Tikal.Application.Tests:** Unit tests for the application layer
- **Tikal.Domain.Tests:** Unit tests for the domain layer
- **Tikal.Integration:** Integration tests spanning all layers

# Run Tests

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