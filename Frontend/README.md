# Table of Contents

- [Summary](#summary)
- [Start Developing](#start-developing)
  - [Run the project](#run-the-project)
  - [Run unit tests](#run-unit-tests)
  - [Linting](#linting)

# Summary

This project contains the frontend for the Tikal project.

# Start Developing

## Run the project

The frontend is automatically run using Aspire. It is not recommended to run the project directly.

## Debug

To be able to debug you need to setup your favourite IDE to attach itselft to the browser in which you are running the frontend.

A configuration to enable vs code to attach itself to firefox is provided in ".vscode/launch.json".

To configure firefox to enable remote debugging see [here](https://marketplace.visualstudio.com/items?itemName=firefox-devtools.vscode-firefox-debug).

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
