# Table of Contents

- [Summary](#summary)
- [Start Developing](#start-developing)
    - [Run the project](#run-the-project)
    - [Run unit tests](#run-unit-tests)
    - [Linting](#linting)

# Summary

This project contains the frontend for the Tikal project.

# Start Developing

This project is made to run using devcontainers. See [devcontainers](https://containers.dev/) for more information.

For devcontainer configuration see the following file:

```
.devcontainer/devcontainer.json
```

If you run this project via devcontainers it will automatically spin up the whole development environment.

The development environment is defined in the following files:

```
docker-compose.yml
```

```
docker-compose.override.yml
```

## Run the project

You can run the project using the following command:

```
npm start
```

How reload is enabled.

## Run unit tests

You can run the unit tests witht he following command:

```
npm run test
```

By default the test use Firefox. You can run the tests on a browser of you choice by opening the following url: http://localhost:9876.

## Linting

To apply the linter run the following command:

```
npm run lint
```

The configuration is defined in **eslint.config.js**

