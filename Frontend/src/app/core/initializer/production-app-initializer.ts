import { inject } from "@angular/core";
import { catchError, firstValueFrom, map, Observable, of } from "rxjs";
import { ActiveAccountStore } from "../../features/authentication/stores/active-account/active-account-store";
import { GetCurrentAccountUseCase } from "../../features/authentication/usecases/get-current-account/get-current-account-usecase";
import { Result } from "neverthrow";
import { Account } from "../../features/authentication/models/account";
import { GetCurrentAccountError } from "../../features/authentication/usecases/get-current-account/get-current-account-errors";
import { TraceService } from "@sentry/angular";

export const productionAppInitializer = (): Observable<unknown> | Promise<unknown> => {
  inject(TraceService);
  const activeAccountStore = inject(ActiveAccountStore);
  const getAccount = inject(GetCurrentAccountUseCase);

  return firstValueFrom(
    getAccount.call().pipe(
      map((result: Result<Account, GetCurrentAccountError>) => {
        if (result.isOk()) {
          activeAccountStore.setAccount(result.value);
        }
      }),
      catchError(() => {
        return of(null);
      }),
    ),
  );
};
