import { inject, Injectable } from "@angular/core";
import { Err, err, Ok, ok, Result } from "neverthrow";
import { LoginDto } from "./dtos/login-dto";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, map, Observable, throwError } from "rxjs";
import { LoginError } from "./login-error";

@Injectable({
  providedIn: "root",
})
export class LoginUseCase {
  private readonly httpClient: HttpClient = inject(HttpClient);

  call(username: string, password: string): Observable<Result<string, LoginError>> {
    const loginDto: LoginDto = {
      username: username,
      password: password,
    };

    const request = this.httpClient.post<TokenDto>("auth:/login", loginDto).pipe(
      map((tokenDto: TokenDto) => {
        return this.handleSucces(tokenDto);
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleFailure(error);
      }),
    );

    return request;
  }

  private handleSucces(tokenDto: TokenDto): Ok<string, never> {
    return ok(tokenDto.accessToken);
  }

  private handleFailure(error: HttpErrorResponse): Err<never, LoginError> | Observable<never> {
    if (error.status == 401) {
      return err(LoginError.InvalidCredentials);
    }

    return throwError(() => error);
  }
}
