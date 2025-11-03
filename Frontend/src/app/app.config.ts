import {
  ApplicationConfig,
  ErrorHandler,
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
import { refreshInterceptor } from "./core/interceptors/refresh/refresh-interceptor";
import { ProductionErroHandler } from "./core/error-handler/production-error-handler";
import { productionAppInitializer } from "./core/initializer/production-app-initializer";
import { sentryInterceptor } from "./core/interceptors/sentry/sentry-interceptor";

export const appConfig: ApplicationConfig = environment.is_production ? getProductionConfig() : getDevelopmentConfig();

function getProductionConfig(): ApplicationConfig {
  return {
    providers: [
      provideBrowserGlobalErrorListeners(),
      provideZonelessChangeDetection(),
      provideRouter(routes, withViewTransitions()),
      provideHttpClient(
        withInterceptors([
          baseUrlInterceptor,
          sentryInterceptor,
          timeoutInterceptor,
          refreshInterceptor,
          authenticationInterceptor,
        ]),
        withFetch(),
      ),
      {
        provide: ErrorHandler,
        useClass: ProductionErroHandler,
      },
      {
        provide: Sentry.TraceService,
        deps: [Router],
      },
      provideAppInitializer(productionAppInitializer),
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
