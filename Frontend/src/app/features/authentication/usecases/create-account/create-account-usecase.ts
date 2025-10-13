import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { CreateAccountError } from "./create-account-errors";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { AccountStore } from "../../stores/account/account-store";
import { AccountDto } from "../../../../shared/dtos/account-dto";
import { catchError, firstValueFrom, map, Observable, throwError } from "rxjs";
import { Account } from "../../models/account";

@Injectable({
  providedIn: "root",
})
export class CreateAccountUseCase extends UseCase<
  [string],
  void,
  CreateAccountError
> {
  protected override name = "Create Account";

  private readonly httpClient: HttpClient = inject(HttpClient);

  private readonly accountStore: AccountStore = inject(AccountStore);

  override async inner(
    name: string,
  ): Promise<Result<void, CreateAccountError>> {
    const createAccountDto: AccountDto = {
      name: name,
    };

    const request = this.httpClient
      .post<AccountDto>("main:/accounts", createAccountDto)
      .pipe(
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
  ): Err<never, CreateAccountError> | Observable<never> {
    if (error.status == 409) {
      return err(CreateAccountError.AccountExists);
    }

    return throwError(() => error);
  }
}
