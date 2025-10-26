import { signalStore, withState } from "@ngrx/signals";
import { withTokenReducer } from "./token-reducers";
import { withTokenEffects } from "./token-effects";

export enum TokenStatus {
  initial,
  loading,
  success,
  failure,
  unauthorized,
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
  withState(initialState),
  withTokenReducer(),
  withTokenEffects(),
);
