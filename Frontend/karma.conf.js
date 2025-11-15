// Karma configuration file, see link for more information
// https://karma-runner.github.io/1.0/config/configuration-file.html
import { fileURLToPath } from "node:url";
import path from "node:path";

import jasmine from "karma-jasmine";
import firefoxLauncher from "karma-firefox-launcher";
import jasmineHtmlReporter from "karma-jasmine-html-reporter";
import coverage from "karma-coverage";

// Recreate __dirname in ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default function config(config) {
  config.set({
    basePath: "",
    frameworks: ["jasmine"],
    plugins: [jasmine, firefoxLauncher, jasmineHtmlReporter, coverage],
    client: {
      jasmine: {
        // you can add configuration options for Jasmine here
      },
    },
    jasmineHtmlReporter: {
      suppressAll: true, // removes duplicated traces
    },
    coverageReporter: {
      dir: path.join(__dirname, "./coverage/frontend"),
      subdir: ".",
      reporters: [{ type: "html" }, { type: "lcov" }, { type: "text-summary" }],
    },
    reporters: ["progress", "kjhtml"],
    browsers: ["Firefox"],
    restartOnFileChange: true,
  });
}
