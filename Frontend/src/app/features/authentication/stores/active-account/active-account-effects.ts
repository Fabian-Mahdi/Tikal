import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { EventInstance, Events, withEffects } from "@ngrx/signals/events";
import { GetCurrentAccountUseCase } from "../../usecases/set-current-account/get-current-account-usecase";
import { GetCurrentAccountError } from "../../usecases/set-current-account/get-current-account-errors";
import { Account } from "../../models/account";
import { Result } from "neverthrow";
import { catchError, map, Observable, of, switchMap, tap } from "rxjs";
import { activeAccountApiEvents } from "./events/active-account-api-events";
import { Router } from "@angular/router";
import { ErrorHandler, inject } from "@angular/core";
import { signalStoreFeature, SignalStoreFeature } from "@ngrx/signals";

const getAccount = (
  events: Events,
  getCurrentAccount: GetCurrentAccountUseCase,
): Observable<EventInstance<string, Account | unknown | void>> =>
  events.on(activeAccountHomeEvents.getAccount).pipe(
    switchMap(() =>
      getCurrentAccount.call().pipe(
        map((result: Result<Account, GetCurrentAccountError>) => {
          if (result.isOk()) {
            return activeAccountApiEvents.accountFound(result.value);
          }
          return activeAccountApiEvents.noAccount();
        }),
        catchError((error) => of(activeAccountApiEvents.loadingFailed(error))),
      ),
    ),
  );

const loadingFailed = (events: Events, errorHandler: ErrorHandler): Observable<EventInstance<string, unknown>> =>
  events.on(activeAccountApiEvents.loadingFailed).pipe(tap((event) => errorHandler.handleError(event.payload)));

const accountFound = (events: Events, router: Router): Observable<EventInstance<string, Account>> =>
  events.on(activeAccountApiEvents.accountFound).pipe(tap(() => router.navigate(["lobbies"])));

export function withActiveAccountEffects(): SignalStoreFeature {
  return signalStoreFeature(
    withEffects(
      (
        store,
        events = inject(Events),
        errorHandler = inject(ErrorHandler),
        getCurrentAccount = inject(GetCurrentAccountUseCase),
        router = inject(Router),
      ) => ({
        getAccount: getAccount(events, getCurrentAccount),
        accountFound: accountFound(events, router),
        loadingFailed: loadingFailed(events, errorHandler),
      }),
    ),
  );
}
