import { Injectable } from "@angular/core";
import { Account } from "../../models/account";

@Injectable({
  providedIn: "root",
})
export class AccountStore {
  CurrentAccount: Account | null = null;

  isLoggedIn(): boolean {
    return this.CurrentAccount != null;
  }

  logout(): void {
    this.CurrentAccount = null;
  }
}
