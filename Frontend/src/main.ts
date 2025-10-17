import { bootstrapApplication } from "@angular/platform-browser";
import { appConfig } from "./app/app.config";
import { App } from "./app/app";
import { environment } from "./environments/environment";
import * as Sentry from "@sentry/angular";

if (environment.is_production) {
  Sentry.init({
    dsn: "https://fb4f68a33279e1e094f2a1b26f72f09b@o4509829949227008.ingest.de.sentry.io/4509832324251728",
    // Setting this option to true will send default PII data to Sentry.
    // For example, automatic IP address collection on events
    sendDefaultPii: true,
    integrations: [Sentry.browserTracingIntegration()],
    // Tracing
    tracesSampleRate: 1, //  Capture 100% of the transactions
    // Set 'tracePropagationTargets' to control for which URLs distributed tracing should be enabled
    tracePropagationTargets: ["auth.tikalonline.com"],
    // Enable sending logs to Sentry
    enableLogs: true,
  });
}

try {
  await bootstrapApplication(App, appConfig);
} catch (err: unknown) {
  console.log(err);
}
