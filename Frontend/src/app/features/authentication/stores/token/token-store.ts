import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class TokenStore {
  AccessToken = "";

  clearToken(): void {
    this.AccessToken = "";
  }
}
