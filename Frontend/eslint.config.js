import { defineConfig } from "eslint/config";
import tseslint from "typescript-eslint";
import angular from "angular-eslint";
import eslintConfigPrettier from "eslint-config-prettier";
import eslint from "@eslint/js";

export default defineConfig([
  // TypeScript files
  {
    files: ["**/*.ts"],
    languageOptions: {
      parser: tseslint.parser,
      parserOptions: {
        project: ["tsconfig.json"],
        tsconfigRootDir: process.cwd(),
      },
    },
    plugins: {
      "@typescript-eslint": tseslint.plugin,
      "@angular-eslint": angular.plugin,
    },
    processor: angular.processInlineTemplates,
    extends: [
      eslint.configs.recommended,
      tseslint.configs.recommended,
      tseslint.configs.stylistic,
      angular.configs.tsRecommended,
      eslintConfigPrettier,
    ],
    rules: {
      // Angular selector rules
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],

      // Angular best practices
      "@angular-eslint/no-empty-lifecycle-method": "warn",
      "@angular-eslint/prefer-on-push-component-change-detection": "warn",
      "@angular-eslint/prefer-output-readonly": "warn",
      "@angular-eslint/prefer-signals": "warn",
      "@angular-eslint/prefer-standalone": "warn",

      // TypeScript best practices
      "@typescript-eslint/array-type": ["warn"],
      "@typescript-eslint/consistent-indexed-object-style": "off",
      "@typescript-eslint/consistent-type-assertions": "warn",
      "@typescript-eslint/consistent-type-definitions": ["warn", "type"],
      "@typescript-eslint/explicit-function-return-type": "error",
      "@typescript-eslint/explicit-member-accessibility": [
        "error",
        { accessibility: "no-public" },
      ],
      "@typescript-eslint/naming-convention": [
        "warn",
        {
          selector: "variable",
          format: ["camelCase", "UPPER_CASE", "PascalCase"],
        },
      ],
      "@typescript-eslint/no-empty-function": "warn",
      "@typescript-eslint/no-empty-interface": "error",
      "@typescript-eslint/no-explicit-any": "warn",
      "@typescript-eslint/no-inferrable-types": "warn",
      "@typescript-eslint/no-shadow": "warn",
      "@typescript-eslint/no-unused-vars": "warn",

      // Security
      "no-eval": "error",
      "no-implied-eval": "error",
    },
  },

  // HTML templates
  {
    files: ["**/*.html"],
    plugins: {
      "@angular-eslint": angular.plugin,
    },
    extends: [
      angular.configs.templateRecommended,
      angular.configs.templateAccessibility,
    ],
    rules: {
      "@angular-eslint/template/attributes-order": [
        "error",
        {
          alphabetical: true,
          order: [
            "STRUCTURAL_DIRECTIVE",
            "TEMPLATE_REFERENCE",
            "ATTRIBUTE_BINDING",
            "INPUT_BINDING",
            "TWO_WAY_BINDING",
            "OUTPUT_BINDING",
          ],
        },
      ],
      "@angular-eslint/template/button-has-type": "warn",
      "@angular-eslint/template/cyclomatic-complexity": [
        "warn",
        { maxComplexity: 10 },
      ],
      "@angular-eslint/template/eqeqeq": "error",
      "@angular-eslint/template/prefer-control-flow": "error",
      "@angular-eslint/template/prefer-ngsrc": "warn",
      "@angular-eslint/template/prefer-self-closing-tags": "warn",
      "@angular-eslint/template/use-track-by-function": "warn",
    },
  },
]);
