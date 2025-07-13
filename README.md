# Table of Contents

- [Summary](#summary)
- [Projects](#projects)
- [Start Developing](#start-developing)
    - [Run projects](#run-projects)
    - [Logging](#logging)
    - [Run tests](#run-tests)
    - [Linting and formatting](#linting-and-formatting)

# Summary

This repository contains all projects related to the Tikal project. An attempt to implement the classic
game [Tikal](https://en.wikipedia.org/wiki/Tikal_(board_game)) as a browser game.

The game can be found under <a>https://tikalonline.com</a> (WIP).

# Projects

The repository contains the following projects:

- **IdentityAPI:** The api exclusively responsible for authentication of users. It provides jwt tokens which are used
  for authorization in the other parts of the system.
- **IdentityAPI.Tests:** Contains unit tests for the IdentityAPI.
- **IdentityAPI.Integration:** Contains integration tests for the IdentityAPI.

- **Infrastructure:** Documentation and configuration of the infrastructure on which the system is deployed.

For more details refer to the READMEs in the respective sub folders.

# Start Developing

## Run Projects

This project is developed using docker containers. It is not recommended to try to run the individual projects locally.

The development environment is defined in the following files:

- docker-compose.yml
- docker-compose.override.yml

If you just want to start the development environment manually you can do so with the following command:

<pre>
docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
</pre>

To actually develop and be able to debug you should use an IDE which automates the starting and stopping of the system
and which can attach itself to a given container.

The following configurations are defined in launchSettings.json

- **"Docker Compose - IdentityAPI":** Choose this configuration if you want to develop/debug the IdentityAPI.

It should all work out of the box. For more information on how to use these configurations and develop multi-container
apps using specific IDEs see the following links:

- [Visual Studio 2022](https://learn.microsoft.com/en-us/visualstudio/containers/tutorial-multicontainer?view=vs-2022)
- Rider TODO

## Logging

In the development environment all projects log to a local [Seq](https://datalust.co/seq) instance
in [OpenTelemetry](https://opentelemetry.io/) format. To authorize the projects you will need to create a seq api key
and provide it to the projects via configuration.

- [How to create a Seq Api key](https://docs.datalust.co/docs/api-keys)
- Refer to the specific READMEs of the projects on how to properly provide the key.

Everything will run just fine without configuring seq, but it is recommended if you want centralized logs which can be
analyzed easily.

If the dev env is running the local seq instance can be reached under <a>http://localhost:5341</a>

## Run Tests

To run the tests of a specific project run the following command:

<pre>
dotnet test .\{project_name}\{project_name}.csproj
</pre>

If you want to run all tests in the solution run:

<pre>
dotnet test .\Tikal.sln
</pre>

The integration tests make use of test containers. So make sure to have a docker environment available when executing
integration tests.

## Linting and formatting

To apply the linting and formatting rules to a specific project run the following command:

<pre>
dotnet format .\{project_name}\{project_name}.csproj
</pre>

The rules are defined using .editorconfig files in the respective sub directories.