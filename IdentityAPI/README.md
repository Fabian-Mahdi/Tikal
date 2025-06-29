# Table of Contents
- [Summary](#summary)

# Summary

This project contains the Api exclusively responsible for the authentication of users. It provides jwt tokens which are used for authorization in the other parts of the system.

# Start Developing

## Configuration

All sensetive configuration values during development are stored using **dotnet user-secrets**. To find out more about it see: [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows)

Before running anything you will need to provide the following configuration:

- Database password:

To set the database password run the following command:

<pre>
dotnet user-secrets set "Database:password" "{your password goes here}"
</pre>

- Seq Api key:

For info regarding seq and how to retrieve an Api key see the README in the root.

To set the api key run the following command:

<pre>
dotnet user-secrets set "Logging:OpenTelemetry:ApiKey" "{your api key goes here}"
</pre>

## Run the project

For information on how to run/debug the project please refer to the README in the root. Do not try to run this project locally.