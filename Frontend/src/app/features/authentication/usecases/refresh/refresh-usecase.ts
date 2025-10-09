import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { RefreshError } from "./errors/refresh-error";
import { err, Err, ok, Ok, Result } from "neverthrow";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { TokenDto } from "../../../../shared/dtos/token-dto";
import { catchError, firstValueFrom, map } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class RefreshUseCase extends UseCase<[], void, RefreshError> {
  protected override name = "Refresh";

  private readonly httpClient: HttpClient = inject(HttpClient);

  override async inner(): Promise<Result<void, RefreshError>> {
    const request = this.httpClient.post<TokenDto>("auth:/refresh", "").pipe(
      map(() => {
        return this.handleSuccess();
      }),
      catchError((error: HttpErrorResponse) => {
        return this.handleFailure(error);
      }),
    );

    return await firstValueFrom(request);
  }

  private handleSuccess(): Ok<void, never> {
    return ok();
  }

  private handleFailure(error: HttpErrorResponse): Err<never, RefreshError> {
    if (error.status == 401) {
      return err(RefreshError.InvalidRefreshToken);
    }

    return err(RefreshError.UnknownError);
  }
}
