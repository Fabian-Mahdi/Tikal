import { HttpErrorResponse, HttpEvent, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { RefreshUseCase } from "../../../features/authentication/usecases/refresh/refresh-usecase";
import { catchError, Observable, switchMap, throwError } from "rxjs";
import { TokenStore } from "../../../features/authentication/stores/token/token-store";
import { Result } from "neverthrow";
import { RefreshError } from "../../../features/authentication/usecases/refresh/refresh-error";
import { environment } from "../../../../environments/environment";

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.startsWith(environment.apis.auth)) {
    return next(req);
  }

  const tokenStore = inject(TokenStore);
  const refresh = inject(RefreshUseCase);
  let isRefreshed = false;

  const attemptRefresh = (originalError: unknown): Observable<HttpEvent<unknown>> => {
    isRefreshed = true;

    return refresh.call().pipe(
      switchMap((result: Result<string, RefreshError>) => {
        if (result.isErr()) {
          return throwError(() => originalError);
        }

        tokenStore.setToken(result.value);
        return next(req);
      }),
    );
  };

  return next(req).pipe(
    catchError((error) => {
      if (!isRefreshed && error instanceof HttpErrorResponse && error.status === 401) {
        return attemptRefresh(error);
      }

      return throwError(() => error);
    }),
  );
};
