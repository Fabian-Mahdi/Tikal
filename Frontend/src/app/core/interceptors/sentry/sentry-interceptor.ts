import { HttpErrorResponse, HttpEvent, HttpInterceptorFn, HttpResponse } from "@angular/common/http";
import { startSpanManual } from "@sentry/angular";
import { finalize, tap } from "rxjs";

export const sentryInterceptor: HttpInterceptorFn = (req, next) => {
  return startSpanManual(
    {
      name: req.url,
      op: "http.client",
    },
    (span) => {
      return next(req).pipe(
        tap({
          next: (event: HttpEvent<unknown>) => {
            if (event instanceof HttpResponse) {
              span.setAttribute("status_code", event.status);
            }
          },
          error: (error: unknown) => {
            if (error instanceof HttpErrorResponse) {
              span.setAttribute("status_code", error.status);
            }
          },
        }),
        finalize(() => {
          span.end();
        }),
      );
    },
  );
};

