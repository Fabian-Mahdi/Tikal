import { inject } from "@angular/core";
import { CanActivateFn, RedirectCommand, Router } from "@angular/router";
import { ActiveAccountStore } from "../../../features/authentication/stores/active-account/active-account-store";

export const isAuthenticated: CanActivateFn = () => {
  const router = inject(Router);
  const activeAccountStore = inject(ActiveAccountStore);

  if (activeAccountStore.isLoggedIn()) {
    return true;
  }

  const homePath = router.parseUrl("");
  return new RedirectCommand(homePath);
};
