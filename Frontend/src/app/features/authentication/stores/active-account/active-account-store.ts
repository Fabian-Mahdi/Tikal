import { signalStore, withComputed, withState } from "@ngrx/signals";
import { Account } from "../../models/account";
import { computed } from "@angular/core";
import { withActiveAccountReducer } from "./active-account-reducers";
import { withActiveAccountEffects } from "./active-account-effects";

export enum ActiveAccountStatus {
  initial,
  loading,
  success,
  failure,
  noAccount,
}

type ActiveAccountState = {
  activeAccount: Account | null;
  status: ActiveAccountStatus;
};

const initialState: ActiveAccountState = {
  activeAccount: null,
  status: ActiveAccountStatus.initial,
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
