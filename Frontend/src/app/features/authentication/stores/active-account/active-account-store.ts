import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { Account } from "../../models/account";
import { computed } from "@angular/core";
import { withActiveAccountReducer } from "./active-account-reducers";
import { withActiveAccountEffects } from "./active-account-effects";
import { withDevtools } from "@angular-architects/ngrx-toolkit";

export enum AccountLoadingStatus {
  initial = "initial",
  loading = "loading",
  success = "success",
  failure = "failure",
  noAccount = "noAccount",
}

export enum AccountCreationStatus {
  inital = "initial",
  loading = "loading",
  success = "success",
  failure = "failure",
  duplicateAccount = "duplicateAccount",
}

type ActiveAccountState = {
  activeAccount: Account | null;
  loadingStatus: AccountLoadingStatus;
  creationStatus: AccountCreationStatus;
};

const initialState: ActiveAccountState = {
  activeAccount: null,
  loadingStatus: AccountLoadingStatus.initial,
  creationStatus: AccountCreationStatus.inital,
};

export const ActiveAccountStore = signalStore(
  { providedIn: "root" },
  withDevtools("active account"),
  withState(initialState),
  withComputed((store) => ({
    isLoggedIn: computed(() => store.activeAccount() != null),
  })),
  withMethods((store) => ({
    setAccount(account: Account): void {
      patchState(store, () => ({ activeAccount: account, loadingStatus: AccountLoadingStatus.success }));
    },
  })),
  withActiveAccountReducer(),
  withActiveAccountEffects(),
);
