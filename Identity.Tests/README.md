# Table of Contents

- [Summary](#summary)
- [Projects](#projects)
- [Run Tests](#run-tests)

# Summary

This directory contains all tests for the Identity server.

# Projects

It contains the following projects:

- **Identity.Application.Tests:** Unit tests for the application layer
- **Identity.Integration:** Integration tests spanning all layers

There are no tests for the domain layer since there is no domain logic to test.

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