import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { SetCurrentAccountError } from "./set-current-account-errors";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { AccountStore } from "../../stores/account/account-store";
import { AccountDto } from "../../../../shared/dtos/account-dto";
import { catchError, firstValueFrom, map, Observable, throwError } from "rxjs";
import { Account } from "../../models/account";

@Injectable({
  providedIn: "root",
})
export class SetCurrentAccountUseCase extends UseCase<
  [],
  void,
  SetCurrentAccountError
> {
  protected override name = "Retrieve Account";

  private readonly httpClient: HttpClient = inject(HttpClient);

  private readonly accountStore: AccountStore = inject(AccountStore);

  override async inner(): Promise<Result<void, SetCurrentAccountError>> {
    const request = this.httpClient.get<AccountDto>("main:/accounts").pipe(
      map((accountDto: AccountDto) => {
        return this.handleSuccess(accountDto);
      }),
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          return this.handleFailure(error);
        }

        return throwError(() => error);
      }),
    );

    return await firstValueFrom(request);
  }

  private handleSuccess(accountDto: AccountDto): Ok<void, never> {
    const account: Account = {
      username: accountDto.name,
    };

    this.accountStore.CurrentAccount = account;

    return ok();
  }

  private handleFailure(
    error: HttpErrorResponse,
  ): Err<never, SetCurrentAccountError> | Observable<never> {
    if (error.status == 404) {
      return err(SetCurrentAccountError.NoAccount);
    }

    return throwError(() => error);
  }
}
