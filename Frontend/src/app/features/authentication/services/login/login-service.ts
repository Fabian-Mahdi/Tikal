import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { User } from "../../models/user";
import { catchError, map, Observable, of } from "rxjs";
import {
  InvalidCredentials,
  LoginError,
  UnknownError,
} from "./errors/login-errors";
import { Failure, Result, Success } from "../../../../shared/utils/result";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { LoginDto } from "./dtos/login-dto";

@Injectable({
  providedIn: "root",
})
export class LoginService {
  private readonly httpClient: HttpClient = inject(HttpClient);

  login(user: User, password: string): Observable<Result<void, LoginError>> {
    const loginDto: LoginDto = {
      username: user.username,
      password: password,
    };

    return this.httpClient.post<TokenDto>("login", loginDto).pipe(
      map((response: TokenDto) => {
        localStorage.setItem("tikalAccessToken", response.accessToken);
        return new Success(undefined);
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status == 401) {
          return of(new Failure(new InvalidCredentials()));
        }
        return of(new Failure(new UnknownError()));
      }),
    );
  }
}
