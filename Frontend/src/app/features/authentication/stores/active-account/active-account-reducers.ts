import { on, withReducer } from "@ngrx/signals/events";
import { activeAccountApiEvents } from "./events/active-account-api-events";
import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { AccountCreationStatus, AccountLoadingStatus } from "./active-account-store";
import { SignalStoreFeature, signalStoreFeature } from "@ngrx/signals";
import { activeAccountCreateEvents } from "./events/active-account-create-events";
import { globalEvents } from "../../../../core/events/global-events";

// Create
const CreateAccount = on(activeAccountCreateEvents.createAccount, () => ({
  creationStatus: AccountCreationStatus.loading,
}));

const AccountCreated = on(activeAccountApiEvents.accountCreated, ({ payload: account }) => ({
  creationStatus: AccountCreationStatus.success,
  activeAccount: account,
}));

const DuplicateAccount = on(activeAccountApiEvents.duplicateAccount, () => ({
  creationStatus: AccountCreationStatus.duplicateAccount,
}));

const CreationError = on(activeAccountApiEvents.createError, () => ({
  creationStatus: AccountCreationStatus.failure,
}));

// Read
const LoadAccount = on(activeAccountHomeEvents.loadAccount, () => ({
  loadingStatus: AccountLoadingStatus.loading,
}));

const AccountLoaded = on(activeAccountApiEvents.accountLoaded, ({ payload: account }) => ({
  loadingStatus: AccountLoadingStatus.success,
  activeAccount: account,
}));

const NoAccount = on(activeAccountApiEvents.noAccount, () => ({
  loadingStatus: AccountLoadingStatus.noAccount,
  activeAccount: null,
}));

const LoadingError = on(activeAccountApiEvents.loadError, () => ({
  loadingStatus: AccountLoadingStatus.failure,
}));

const ClearAccount = on(globalEvents.logout, () => ({
  loadingStatus: AccountLoadingStatus.initial,
  activeAccount: null,
}));

export function withActiveAccountReducer(): SignalStoreFeature {
  return signalStoreFeature(
    withReducer(
      // Create
      CreateAccount,
      AccountCreated,
      DuplicateAccount,
      CreationError,
      // Read
      LoadAccount,
      AccountLoaded,
      NoAccount,
      LoadingError,
      ClearAccount,
    ),
  );
}
