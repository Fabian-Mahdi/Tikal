import { inject, Injectable } from "@angular/core";
import { RefreshError } from "./refresh-error";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, map, Observable, throwError } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class RefreshUseCase {
  private readonly httpClient: HttpClient = inject(HttpClient);

  call(): Observable<Result<string, RefreshError>> {
    const request = this.httpClient.post<TokenDto>("auth:/refresh", null).pipe(
      map((tokenDto: TokenDto) => {
        return this.handleSuccess(tokenDto);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleFailure(error);
      }),
    );

    return request;
  }

  private handleSuccess(tokenDto: TokenDto): Ok<string, never> {
    return ok(tokenDto.accessToken);
  }

  private handleFailure(error: HttpErrorResponse): Err<never, RefreshError> | Observable<never> {
    if (error.status == 401) {
      return err(RefreshError.InvalidRefreshToken);
    }

    return throwError(() => error);
  }
}
