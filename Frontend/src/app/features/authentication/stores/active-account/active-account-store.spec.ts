import { ErrorHandler } from "@angular/core";
import { GetCurrentAccountUseCase } from "../../usecases/get-current-account/get-current-account-usecase";
import { CreateAccountUseCase } from "../../usecases/create-account/create-account-usecase";
import { TestBed } from "@angular/core/testing";
import { AccountCreationStatus, AccountLoadingStatus, ActiveAccountStore } from "./active-account-store";
import { Dispatcher } from "@ngrx/signals/events";
import { activeAccountCreateEvents } from "./events/active-account-create-events";
import { of, throwError } from "rxjs";
import { err, ok } from "neverthrow";
import { accountTestData } from "../../test-data/models/account-test-data";
import { CreateAccountError } from "../../usecases/create-account/create-account-errors";
import { errorTestData } from "../../../../shared/test-data/misc/error-test-data";
import { activeAccountHomeEvents } from "./events/active-account-home-events";
import { GetCurrentAccountError } from "../../usecases/get-current-account/get-current-account-errors";
import { globalEvents } from "../../../../core/events/global-events";
import { beforeEach, describe, expect, it, vi } from "vitest";

class MockErrorHandler {
  handleError = vi.fn();
}

class MockGetCurrentAccountUseCase {
  call = vi.fn();
}

class MockCreateAccountUseCase {
  call = vi.fn();
}

describe("ActiveAccountStore", () => {
  // dependencies
  let errorHandler: MockErrorHandler;
  let getCurrentAccountUseCase: MockGetCurrentAccountUseCase;
  let createAccountUseCase: MockCreateAccountUseCase;

  // under test
  let dispatch: Dispatcher;

  beforeEach(() => {
    errorHandler = new MockErrorHandler();
    getCurrentAccountUseCase = new MockGetCurrentAccountUseCase();
    createAccountUseCase = new MockCreateAccountUseCase();

    TestBed.configureTestingModule({
      providers: [
        {
          provide: ErrorHandler,
          useValue: errorHandler,
        },
        {
          provide: GetCurrentAccountUseCase,
          useValue: getCurrentAccountUseCase,
        },
        {
          provide: CreateAccountUseCase,
          useValue: createAccountUseCase,
        },
      ],
    });

    dispatch = TestBed.inject(Dispatcher);
  });

  for (const account of accountTestData) {
    it("[CreateAccountEvent] should call createAccountUseCase", () => {
      TestBed.inject(ActiveAccountStore);
      createAccountUseCase.call.mockReturnValue(of(ok(account)));

      dispatch.dispatch(activeAccountCreateEvents.createAccount(account.username));

      expect(createAccountUseCase.call.mock.calls.length).toBe(1);
      expect(createAccountUseCase.call).toHaveBeenCalledWith(account.username);
    });
  }

  for (const account of accountTestData) {
    it("[CreateAccountEvent] should set the expected account if createAccountUseCase succeeds", () => {
      const store = TestBed.inject(ActiveAccountStore);
      createAccountUseCase.call.mockReturnValue(of(ok(account)));

      dispatch.dispatch(activeAccountCreateEvents.createAccount(account.username));

      expect(store.creationStatus()).toBe(AccountCreationStatus.success);
      expect(store.activeAccount()).toBe(account);
    });
  }

  for (const account of accountTestData) {
    it("[CreateAccountEvent] should set AccountCreationError if createAccountUseCase fails", () => {
      const store = TestBed.inject(ActiveAccountStore);
      createAccountUseCase.call.mockReturnValue(of(err(CreateAccountError.AccountExists)));

      dispatch.dispatch(activeAccountCreateEvents.createAccount(account.username));

      expect(store.creationStatus()).toBe(AccountCreationStatus.duplicateAccount);
    });
  }

  for (const error of errorTestData) {
    it("[CreateAccountEvent] should call errorHandler if createAccountUseCase throws", () => {
      const store = TestBed.inject(ActiveAccountStore);
      createAccountUseCase.call.mockReturnValue(throwError(() => error));

      dispatch.dispatch(activeAccountCreateEvents.createAccount("name"));

      expect(store.creationStatus()).toBe(AccountCreationStatus.failure);
      expect(errorHandler.handleError.mock.calls.length).toBe(1);
      expect(errorHandler.handleError).toHaveBeenCalledWith(error);
    });
  }

  for (const account of accountTestData) {
    it("[LoadAccountEvent] should call getCurrentAccountUseCase", () => {
      TestBed.inject(ActiveAccountStore);
      getCurrentAccountUseCase.call.mockReturnValue(of(ok(account)));

      dispatch.dispatch(activeAccountHomeEvents.loadAccount());

      expect(getCurrentAccountUseCase.call.mock.calls.length).toBe(1);
      expect(getCurrentAccountUseCase.call).toHaveBeenCalledWith();
    });
  }

  for (const account of accountTestData) {
    it("[LoadAccountEvent] should set the expected account if getCurrentAccountUseCase succeeds", () => {
      const store = TestBed.inject(ActiveAccountStore);
      getCurrentAccountUseCase.call.mockReturnValue(of(ok(account)));

      dispatch.dispatch(activeAccountHomeEvents.loadAccount());

      expect(store.loadingStatus()).toBe(AccountLoadingStatus.success);
      expect(store.activeAccount()).toBe(account);
    });
  }

  it("[LoadAccountEvent] should set AccountLoadingError if getCurrentAccountUseCase fails", () => {
    const store = TestBed.inject(ActiveAccountStore);
    getCurrentAccountUseCase.call.mockReturnValue(of(err(GetCurrentAccountError.NoAccount)));

    dispatch.dispatch(activeAccountHomeEvents.loadAccount());

    expect(store.loadingStatus()).toBe(AccountLoadingStatus.noAccount);
  });

  for (const error of errorTestData) {
    it("[LoadAccountEvent] should call errorHandler if getCurrentAcountUseCase throws", () => {
      const store = TestBed.inject(ActiveAccountStore);
      getCurrentAccountUseCase.call.mockReturnValue(throwError(() => error));

      dispatch.dispatch(activeAccountHomeEvents.loadAccount());

      expect(store.loadingStatus()).toBe(AccountLoadingStatus.failure);
      expect(errorHandler.handleError.mock.calls.length).toBe(1);
      expect(errorHandler.handleError).toBeCalledWith(error);
    });
  }

  for (const account of accountTestData) {
    it("should return true for IsLoggedIn when account is set", () => {
      const store = TestBed.inject(ActiveAccountStore);

      store.setAccount(account);

      expect(store.isLoggedIn()).toBe(true);
    });
  }

  it("should return false for IsLoggedIn when account is not set", () => {
    const store = TestBed.inject(ActiveAccountStore);

    expect(store.isLoggedIn()).toBe(false);
  });

  for (const account of accountTestData) {
    it("[Logout] should unset active account and loading status", () => {
      const store = TestBed.inject(ActiveAccountStore);

      store.setAccount(account);

      dispatch.dispatch(globalEvents.logout());

      expect(store.activeAccount()).toBeNull();
      expect(store.loadingStatus()).toBe(AccountLoadingStatus.initial);
    });
  }
});
