import { signalStore, withComputed, withState } from "@ngrx/signals";
import { Account } from "../../models/account";
import { computed } from "@angular/core";
import { withActiveAccountReducer } from "./active-account-reducers";
import { withActiveAccountEffects } from "./active-account-effects";

export enum AccountLoadingStatus {
  initial,
  loading,
  success,
  failure,
  noAccount,
}

export enum AccountCreationStatus {
  inital,
  loading,
  success,
  failure,
  duplicateAccount,
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
  withState(initialState),
  withComputed((store) => ({
    isLoggedIn: computed(() => store.activeAccount != null),
  })),
  withActiveAccountReducer(),
  withActiveAccountEffects(),
);
