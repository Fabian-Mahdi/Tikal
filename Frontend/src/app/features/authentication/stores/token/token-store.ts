import { signalStore, withState } from "@ngrx/signals";
import { withTokenReducer } from "./token-reducers";
import { withTokenEffects } from "./token-effects";
import { withDevtools } from "@angular-architects/ngrx-toolkit";

export enum TokenStatus {
  initial = "initial",
  loading = "loading",
  success = "success",
  failure = "failure",
  unauthorized = "unauthorized",
}

type TokenState = {
  token: string;
  status: TokenStatus;
};

const initialState: TokenState = {
  token: "",
  status: TokenStatus.initial,
};

export const TokenStore = signalStore(
  { providedIn: "root" },
  withDevtools("token"),
  withState(initialState),
  withTokenReducer(),
  withTokenEffects(),
);
