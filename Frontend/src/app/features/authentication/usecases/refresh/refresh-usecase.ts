import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { RefreshError } from "./errors/refresh-error";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, firstValueFrom, map, Observable, throwError } from "rxjs";
import { TokenStore } from "../../stores/token/token-store";

@Injectable({
  providedIn: "root",
})
export class RefreshUseCase extends UseCase<[], void, RefreshError> {
  protected override name = "Refresh";

  private readonly httpClient: HttpClient = inject(HttpClient);

  private readonly tokenStore: TokenStore = inject(TokenStore);

  override async inner(): Promise<Result<void, RefreshError>> {
    const request = this.httpClient.post<TokenDto>("auth:/refresh", "").pipe(
      map((tokenDto: TokenDto) => {
        return this.handleSuccess(tokenDto);
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

  private handleSuccess(tokenDto: TokenDto): Ok<void, never> {
    this.tokenStore.AccessToken = tokenDto.accessToken;

    return ok();
  }

  private handleFailure(
    error: HttpErrorResponse,
  ): Err<never, RefreshError> | Observable<never> {
    if (error.status == 401) {
      return err(RefreshError.InvalidRefreshToken);
    }

    return throwError(() => error);
  }
}
