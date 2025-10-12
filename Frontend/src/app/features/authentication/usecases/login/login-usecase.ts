import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { Err, err, Ok, ok, Result } from "neverthrow";
import { LoginDto } from "./dtos/login-dto";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, firstValueFrom, map, Observable, throwError } from "rxjs";
import { LoginError } from "./erros/login-error";
import { TokenStore } from "../../stores/token/token-store";

@Injectable({
  providedIn: "root",
})
export class LoginUseCase extends UseCase<[string, string], void, LoginError> {
  protected override name = "Login";

  private readonly httpClient: HttpClient = inject(HttpClient);

  private readonly tokenStore: TokenStore = inject(TokenStore);

  override async inner(
    username: string,
    password: string,
  ): Promise<Result<void, LoginError>> {
    const loginDto: LoginDto = {
      username: username,
      password: password,
    };

    const request = this.httpClient
      .post<TokenDto>("auth:/login", loginDto)
      .pipe(
        map((tokenDto: TokenDto) => {
          return this.handleSucces(tokenDto);
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

  private handleSucces(tokenDto: TokenDto): Ok<void, never> {
    this.tokenStore.AccessToken = tokenDto.accessToken;

    return ok();
  }

  private handleFailure(
    error: HttpErrorResponse,
  ): Err<never, LoginError> | Observable<never> {
    if (error.status == 401) {
      return err(LoginError.InvalidCredentials);
    }

    return throwError(() => error);
  }
}
