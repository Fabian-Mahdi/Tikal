import {
  ApplicationConfig,
  ErrorHandler,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from "@angular/core";
import { provideRouter, withViewTransitions } from "@angular/router";
import { routes } from "./app.routes";
import { provideHttpClient, withFetch, withInterceptors } from "@angular/common/http";
import { baseUrlInterceptor } from "./core/interceptors/base-url/base-url-interceptor";
import { authenticationInterceptor } from "./core/interceptors/authentication/authentication-interceptor";
import { provideInstrumentation } from "./core/telemetry/otel-instrumentation";
import { GlobalErrorHandler } from "./core/error-handler/global-error-handler";
import { appInitializer } from "./core/initializer/app-initializer";
import { refreshInterceptor } from "./core/interceptors/refresh/refresh-interceptor";

export const appConfig: ApplicationConfig = getConfig();

function getConfig(): ApplicationConfig {
  return {
    providers: [
      provideInstrumentation(),
      provideBrowserGlobalErrorListeners(),
      provideRouter(routes, withViewTransitions()),
      provideHttpClient(
        withInterceptors([baseUrlInterceptor, refreshInterceptor, authenticationInterceptor]),
        withFetch(),
      ),
      {
        provide: ErrorHandler,
        useClass: GlobalErrorHandler,
      },
      provideAppInitializer(appInitializer),
    ],
  };
}
