import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { EventInstance, Events, withEffects } from "@ngrx/signals/events";
import { GetCurrentAccountUseCase } from "../../usecases/get-current-account/get-current-account-usecase";
import { GetCurrentAccountError } from "../../usecases/get-current-account/get-current-account-errors";
import { Account } from "../../models/account";
import { Result } from "neverthrow";
import { catchError, map, Observable, of, switchMap, tap } from "rxjs";
import { activeAccountApiEvents } from "./events/active-account-api-events";
import { Router } from "@angular/router";
import { ErrorHandler, inject } from "@angular/core";
import { signalStoreFeature, SignalStoreFeature } from "@ngrx/signals";
import { CreateAccountUseCase } from "../../usecases/create-account/create-account-usecase";
import { activeAccountCreateEvents } from "./events/active-account-create-events";
import { CreateAccountError } from "../../usecases/create-account/create-account-errors";

const createAccount = (
  events: Events,
  createUserAccount: CreateAccountUseCase,
): Observable<EventInstance<string, unknown>> =>
  events.on(activeAccountCreateEvents.createAccount).pipe(
    switchMap((event) =>
      createUserAccount.call(event.payload).pipe(
        map((result: Result<Account, CreateAccountError>) => {
          if (result.isOk()) {
            return activeAccountApiEvents.accountCreated(result.value);
          }

          return activeAccountApiEvents.duplicateAccount();
        }),
        catchError((error) => of(activeAccountApiEvents.createError(error))),
      ),
    ),
  );

const loadAccount = (
  events: Events,
  getCurrentAccount: GetCurrentAccountUseCase,
): Observable<EventInstance<string, unknown>> =>
  events.on(activeAccountHomeEvents.loadAccount).pipe(
    switchMap(() =>
      getCurrentAccount.call().pipe(
        map((result: Result<Account, GetCurrentAccountError>) => {
          if (result.isOk()) {
            return activeAccountApiEvents.accountLoaded(result.value);
          }
          return activeAccountApiEvents.noAccount();
        }),
        catchError((error) => of(activeAccountApiEvents.loadError(error))),
      ),
    ),
  );

const accountSet = (events: Events, router: Router): Observable<EventInstance<string, Account>> =>
  events
    .on(activeAccountApiEvents.accountLoaded, activeAccountApiEvents.accountCreated)
    .pipe(tap(() => router.navigate([{ outlets: { primary: ["lobbies"], overlay: null } }])));

const noAccount = (events: Events, router: Router): Observable<EventInstance<string, void>> =>
  events
    .on(activeAccountApiEvents.noAccount)
    .pipe(tap(() => router.navigate([{ outlets: { overlay: "createaccount" } }])));

const error = (events: Events, errorHandler: ErrorHandler): Observable<EventInstance<string, unknown>> =>
  events
    .on(activeAccountApiEvents.loadError, activeAccountApiEvents.createError)
    .pipe(tap((event) => errorHandler.handleError(event.payload)));

const cancel = (events: Events, router: Router): Observable<EventInstance<string, void>> =>
  events.on(activeAccountCreateEvents.cancel).pipe(tap(() => router.navigate([{ outlets: { overlay: null } }])));

export function withActiveAccountEffects(): SignalStoreFeature {
  return signalStoreFeature(
    withEffects(
      (
        store,
        events = inject(Events),
        errorHandler = inject(ErrorHandler),
        getCurrentAccount = inject(GetCurrentAccountUseCase),
        createUserAccount = inject(CreateAccountUseCase),
        router = inject(Router),
      ) => ({
        createAccount: createAccount(events, createUserAccount),
        loadAccount: loadAccount(events, getCurrentAccount),
        accountSet: accountSet(events, router),
        noAccount: noAccount(events, router),
        error: error(events, errorHandler),
        cancel: cancel(events, router),
      }),
    ),
  );
}
