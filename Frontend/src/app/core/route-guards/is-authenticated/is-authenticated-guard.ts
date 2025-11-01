import { inject } from "@angular/core";
import { ActiveAccountStore } from "../../../features/authentication/stores/active-account/active-account-store";
import { CanActivateFn, Router } from "@angular/router";

export const isAuthenticated: CanActivateFn = (): boolean => {
  const activeAccountStore = inject(ActiveAccountStore);
  const router = inject(Router);

  if (activeAccountStore.isLoggedIn()) {
    return true;
  }

  router.navigate([""], { replaceUrl: true });
  return false;
};
