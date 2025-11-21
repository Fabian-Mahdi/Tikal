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
import { timeoutInterceptor } from "./core/interceptors/timeout/timeout-interceptor";
import { DevelopmentErrorHandler } from "./core/error-handler/development-error-handler";
import { developmentAppInitializer } from "./core/initializer/development-app-initializer";
import { refreshInterceptor } from "./core/interceptors/refresh/refresh-interceptor";

export const appConfig: ApplicationConfig = getDevelopmentConfig();

function getDevelopmentConfig(): ApplicationConfig {
  return {
    providers: [
      provideInstrumentation(),
      provideBrowserGlobalErrorListeners(),
      provideRouter(routes, withViewTransitions()),
      provideHttpClient(
        withInterceptors([baseUrlInterceptor, timeoutInterceptor, refreshInterceptor, authenticationInterceptor]),
        withFetch(),
      ),
      {
        provide: ErrorHandler,
        useClass: DevelopmentErrorHandler,
      },
      provideAppInitializer(developmentAppInitializer),
    ],
  };
}
