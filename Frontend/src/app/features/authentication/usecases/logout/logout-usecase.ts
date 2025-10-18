import { inject, Injectable } from "@angular/core";
import { UseCase } from "../../../../core/usecase/usecase";
import { ok, Result } from "neverthrow";
import { AccountStore } from "../../stores/account/account-store";
import { TokenStore } from "../../stores/token/token-store";

@Injectable({
  providedIn: "root",
})
export class LogoutUseCase extends UseCase<[], void, void> {
  protected override name = "Logout";

  private readonly accountStore: AccountStore = inject(AccountStore);

  private readonly tokenStore: TokenStore = inject(TokenStore);

  override async inner(): Promise<Result<void, void>> {
    this.accountStore.logout();

    this.tokenStore.clearToken();

    return ok();
  }
}
