import { inject } from "@angular/core";
import { ActiveAccountStore } from "../../../features/authentication/stores/active-account/active-account-store";
import { CanActivateFn } from "@angular/router";
import { Dispatcher } from "@ngrx/signals/events";
import { activeAccountHomeEvents } from "../../../features/authentication/stores/active-account/events/active-account-home-events";

export const isAuthenticated: CanActivateFn = (): boolean => {
  const activeAccountStore = inject(ActiveAccountStore);
  const dispatcher = inject(Dispatcher);

  if (!activeAccountStore.isLoggedIn()) {
    dispatcher.dispatch(activeAccountHomeEvents.loadAccount());
  }

  return true;
};
