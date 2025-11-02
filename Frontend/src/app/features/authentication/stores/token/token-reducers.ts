import { on, withReducer } from "@ngrx/signals/events";
import { tokenLoginEvents } from "./events/token-login-events";
import { TokenStatus } from "./token-store";
import { tokenApiEvents } from "./events/token-api-events";
import { signalStoreFeature, SignalStoreFeature } from "@ngrx/signals";

const Login = on(tokenLoginEvents.login, () => ({
  status: TokenStatus.loading,
}));

const Cancel = on(tokenLoginEvents.cancel, () => ({
  status: TokenStatus.initial,
}));

const Authenticated = on(tokenApiEvents.authenticated, (event) => ({
  status: TokenStatus.success,
  token: event.payload,
}));

const AuthenticationFailed = on(tokenApiEvents.authenticationFailed, () => ({
  status: TokenStatus.unauthorized,
  token: "",
}));

const Logout = on(tokenApiEvents.logoutCompleted, () => ({
  status: TokenStatus.initial,
  token: "",
}));

const LoadingError = on(tokenApiEvents.error, () => ({
  status: TokenStatus.failure,
}));

export function withTokenReducer(): SignalStoreFeature {
  return signalStoreFeature(withReducer(Login, Logout, Cancel, Authenticated, AuthenticationFailed, LoadingError));
}
