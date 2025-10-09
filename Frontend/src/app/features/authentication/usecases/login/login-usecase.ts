import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { Err, err, Ok, ok, Result } from "neverthrow";
import { LoginDto } from "./dtos/login-dto";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, firstValueFrom, map } from "rxjs";
import { LoginError } from "./erros/login-error";
import { User } from "../../models/user";

@Injectable({
  providedIn: "root",
})
export class LoginUseCase extends UseCase<[User, string], void, LoginError> {
  protected override name = "Login";

  private readonly httpClient: HttpClient = inject(HttpClient);

  override async inner(
    user: User,
    password: string,
  ): Promise<Result<void, LoginError>> {
    const loginDto: LoginDto = {
      username: user.username,
      password: password,
    };

    const request = this.httpClient
      .post<TokenDto>("auth:/login", loginDto)
      .pipe(
        map(() => {
          return this.handleSucces();
        }),
        catchError((error: HttpErrorResponse) => {
          return this.handleFailure(error);
        }),
      );

    return await firstValueFrom(request);
  }

  private handleSucces(): Ok<void, never> {
    return ok();
  }

  private handleFailure(error: HttpErrorResponse): Err<never, LoginError> {
    if (error.status == 401) {
      return err(LoginError.InvalidCredentials);
    }

    return err(LoginError.UnknownError);
  }
}
