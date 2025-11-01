import { inject } from "@angular/core";
import { ActiveAccountStore } from "../../../features/authentication/stores/active-account/active-account-store";
import { CanActivateFn } from "@angular/router";

export const isNotAuthenticated: CanActivateFn = () => {
  const activeAccountStore = inject(ActiveAccountStore);

  return !activeAccountStore.isLoggedIn();
};
