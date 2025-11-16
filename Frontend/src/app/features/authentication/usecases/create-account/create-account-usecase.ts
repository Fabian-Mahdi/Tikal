import { inject, Injectable } from "@angular/core";
import { CreateAccountError } from "./create-account-errors";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { AccountDto } from "../../../../shared/dtos/account-dto";
import { catchError, map, Observable, throwError } from "rxjs";
import { Account } from "../../models/account";

@Injectable({
  providedIn: "root",
})
export class CreateAccountUseCase {
  private readonly httpClient: HttpClient = inject(HttpClient);

  call(name: string): Observable<Result<Account, CreateAccountError>> {
    const createAccountDto: AccountDto = {
      name: name,
    };

    const request = this.httpClient.post<AccountDto>("main:/accounts", createAccountDto).pipe(
      map((accountDto: AccountDto) => {
        return this.handleSuccess(accountDto);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleFailure(error);
      }),
    );

    return request;
  }

  private handleSuccess(accountDto: AccountDto): Ok<Account, never> {
    const account: Account = {
      username: accountDto.name,
    };

    return ok(account);
  }

  private handleFailure(error: HttpErrorResponse): Err<never, CreateAccountError> | Observable<never> {
    if (error.status == 409) {
      return err(CreateAccountError.AccountExists);
    }

    return throwError(() => error);
  }
}
