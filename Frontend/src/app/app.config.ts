import {
  ApplicationConfig,
  ErrorHandler,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from "@angular/core";
import { provideRouter, Router, withViewTransitions } from "@angular/router";

import * as Sentry from "@sentry/angular";
import { routes } from "./app.routes";
import { provideHttpClient, withFetch, withInterceptors } from "@angular/common/http";
import { baseUrlInterceptor } from "./core/interceptors/base-url/base-url-interceptor";
import { authenticationInterceptor } from "./core/interceptors/authentication/authentication-interceptor";
import { provideInstrumentation } from "./core/telemetry/otel-instrumentation";
import { environment } from "../environments/environment";
import { timeoutInterceptor } from "./core/interceptors/timeout/timeout-interceptor";
import { DevelopmentErrorHandler } from "./core/error-handler/development-error-handler";
import { developmentAppInitializer } from "./core/initializer/development-app-initializer";

export const appConfig: ApplicationConfig = environment.is_production ? getProductionConfig() : getDevelopmentConfig();

function getProductionConfig(): ApplicationConfig {
  return {
    providers: [
      provideBrowserGlobalErrorListeners(),
      provideZonelessChangeDetection(),
      provideRouter(routes),
      provideHttpClient(
        withInterceptors([baseUrlInterceptor, authenticationInterceptor, timeoutInterceptor]),
        withFetch(),
      ),
      {
        provide: ErrorHandler,
        useValue: Sentry.createErrorHandler(),
      },
      {
        provide: Sentry.TraceService,
        deps: [Router],
      },
      provideAppInitializer(() => {
        inject(Sentry.TraceService);
      }),
    ],
  };
}

function getDevelopmentConfig(): ApplicationConfig {
  return {
    providers: [
      provideInstrumentation(),
      provideBrowserGlobalErrorListeners(),
      provideZonelessChangeDetection(),
      provideRouter(routes, withViewTransitions()),
      provideHttpClient(
        withInterceptors([authenticationInterceptor, baseUrlInterceptor, timeoutInterceptor]),
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
