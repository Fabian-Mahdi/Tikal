import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { ok, Result } from "neverthrow";
import { AccountStore } from "../../stores/account/account-store";
import { TokenStore } from "../../stores/token/token-store";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class LogoutUseCase extends UseCase<[], void, void> {
  protected override name = "Logout";

  private readonly accountStore: AccountStore = inject(AccountStore);

  private readonly tokenStore: TokenStore = inject(TokenStore);

  private readonly httpClient: HttpClient = inject(HttpClient);

  override async inner(): Promise<Result<void, void>> {
    this.accountStore.logout();

    this.tokenStore.clearToken();

    const request = this.httpClient.post("auth:/logout", "");

    await firstValueFrom(request);

    return ok();
  }
}
