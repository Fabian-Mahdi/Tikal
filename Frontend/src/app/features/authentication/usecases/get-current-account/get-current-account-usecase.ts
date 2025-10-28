import { inject, Injectable } from "@angular/core";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { AccountDto } from "../../../../shared/dtos/account-dto";
import { catchError, map, Observable, throwError } from "rxjs";
import { Account } from "../../models/account";
import { GetCurrentAccountError } from "./get-current-account-errors";

@Injectable({
  providedIn: "root",
})
export class GetCurrentAccountUseCase {
  private readonly httpClient: HttpClient = inject(HttpClient);

  call(): Observable<Result<Account, GetCurrentAccountError>> {
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

    return request;
  }

  private handleSuccess(accountDto: AccountDto): Ok<Account, never> {
    const account: Account = {
      username: accountDto.name,
    };

    return ok(account);
  }

  private handleFailure(error: HttpErrorResponse): Err<never, GetCurrentAccountError> | Observable<never> {
    if (error.status == 404) {
      return err(GetCurrentAccountError.NoAccount);
    }

    return throwError(() => error);
  }
}
