import { catchError, map, Observable, of, switchMap, tap } from "rxjs";
import { LoginUseCase } from "../../usecases/login/login-usecase";
import { EventInstance, Events, withEffects } from "@ngrx/signals/events";
import { tokenLoginEvents } from "./events/token-login-events";
import { Result } from "neverthrow";
import { LoginError } from "../../usecases/login/login-error";
import { tokenApiEvents } from "./events/token-api-events";
import { ErrorHandler, inject } from "@angular/core";
import { activeAccountHomeEvents } from "../active-account/events/active-account-home-events";
import { signalStoreFeature, SignalStoreFeature } from "@ngrx/signals";
import { Router } from "@angular/router";

const login = (events: Events, loginUser: LoginUseCase): Observable<EventInstance<string, unknown>> =>
  events.on(tokenLoginEvents.login).pipe(
    switchMap((event) =>
      loginUser.call(event.payload.username, event.payload.password).pipe(
        map((result: Result<string, LoginError>) => {
          if (result.isOk()) {
            return tokenApiEvents.authenticated(result.value);
          }
          return tokenApiEvents.authenticationFailed();
        }),
        catchError((error) => of(tokenApiEvents.error(error))),
      ),
    ),
  );

const error = (events: Events, errorHandler: ErrorHandler): Observable<EventInstance<string, unknown>> =>
  events.on(tokenApiEvents.error).pipe(tap((event) => errorHandler.handleError(event.payload)));

const authenticated = (events: Events): Observable<EventInstance<string, void>> =>
  events.on(tokenApiEvents.authenticated).pipe(map(() => activeAccountHomeEvents.loadAccount()));

const cancel = (events: Events, router: Router): Observable<EventInstance<string, void>> =>
  events.on(tokenLoginEvents.cancel).pipe(tap(() => router.navigate([{ outlets: { overlay: null } }])));

export function withTokenEffects(): SignalStoreFeature {
  return signalStoreFeature(
    withEffects(
      (
        store,
        events = inject(Events),
        errorHandler = inject(ErrorHandler),
        loginUser = inject(LoginUseCase),
        router = inject(Router),
      ) => ({
        login: login(events, loginUser),
        error: error(events, errorHandler),
        authenticated: authenticated(events),
        cancel: cancel(events, router),
      }),
    ),
  );
}
