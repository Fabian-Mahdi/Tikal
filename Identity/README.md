# Table of Contents

- [Summary](#summary)
- [Start Developing](#start-developing)
    - [Run Projects](#run-project)
    - [Linting and Formatting](#linting-and-formatting)

# Summary

This directory contains all projects related to the Identity server.

This application tries to follow clean architecture standards, where every project corresponds to one layer.

# Start Developing

## Run Project

This project is developed using [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview).
It is not recommended to try to run this project directly.

### Linting and formatting

To apply the linting and formatting rules to a specific project run the following command:

<pre>
dotnet format .\{project_name}\{project_name}.csproj
</pre>

The rules are defined using .editorconfig in the root directory.