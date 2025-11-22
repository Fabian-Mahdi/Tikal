import { ErrorHandler } from "@angular/core";
import { LoginUseCase } from "../../usecases/login/login-usecase";
import { LogoutUseCase } from "../../usecases/logout/logout-usecase";
import { Dispatcher, Events } from "@ngrx/signals/events";
import { TestBed } from "@angular/core/testing";
import { TokenStatus, TokenStore } from "./token-store";
import { of, tap, throwError } from "rxjs";
import { err, ok } from "neverthrow";
import { tokenLoginEvents } from "./events/token-login-events";
import { activeAccountHomeEvents } from "../active-account/events/active-account-home-events";
import { LoginError } from "../../usecases/login/login-error";
import { errorTestData } from "../../../../shared/test-data/misc/error-test-data";
import { globalEvents } from "../../../../core/events/global-events";
import { beforeEach, describe, expect, it, vi } from "vitest";

class MockErrorHandler {
  handleError = vi.fn();
}

class MockLoginUseCase {
  call = vi.fn();
}

class MockLogoutUseCase {
  call = vi.fn();
}

describe("TokenStore", () => {
  // data
  const username = "username";
  const password = "password";
  const token =
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";

  // dependencies
  let errorHandler: MockErrorHandler;
  let loginUseCase: MockLoginUseCase;
  let logoutUseCase: MockLogoutUseCase;

  // under test
  let dispatch: Dispatcher;

  beforeEach(() => {
    errorHandler = new MockErrorHandler();
    loginUseCase = new MockLoginUseCase();
    logoutUseCase = new MockLogoutUseCase();

    TestBed.configureTestingModule({
      providers: [
        {
          provide: ErrorHandler,
          useValue: errorHandler,
        },
        {
          provide: LoginUseCase,
          useValue: loginUseCase,
        },
        {
          provide: LogoutUseCase,
          useValue: logoutUseCase,
        },
      ],
    });

    TestBed.inject(TokenStore);
    dispatch = TestBed.inject(Dispatcher);
  });

  it("should set token and status when setToken", () => {
    const store = TestBed.inject(TokenStore);

    store.setToken(token);

    expect(store.token()).toBe(token);
    expect(store.status()).toBe(TokenStatus.success);
  });

  it("[LoginEvent] should call LoginUseCase", () => {
    loginUseCase.call.mockReturnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(loginUseCase.call.mock.calls.length).toBe(1);
    expect(loginUseCase.call).toHaveBeenCalledWith(username, password);
  });

  it("[LoginEvent] should emit loadAccount event if LoginUseCase returns success", () => {
    let loadAccountEmitted = false;

    const event = TestBed.inject(Events);
    event
      .on(activeAccountHomeEvents.loadAccount)
      .pipe(tap(() => (loadAccountEmitted = true)))
      .subscribe();

    loginUseCase.call.mockReturnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(loadAccountEmitted).toBe(true);
  });

  it("[LoginEvent] should set token if LoginUseCase returns success", () => {
    const store = TestBed.inject(TokenStore);

    loginUseCase.call.mockReturnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(store.token()).toBe(token);
    expect(store.status()).toBe(TokenStatus.success);
  });

  it("[LoginEvent] should set TokenError if LoginUseCase returns InvalidCredentials", () => {
    const store = TestBed.inject(TokenStore);

    loginUseCase.call.mockReturnValue(of(err(LoginError.InvalidCredentials)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(store.token()).toBe("");
    expect(store.status()).toBe(TokenStatus.unauthorized);
  });

  for (const error of errorTestData) {
    it("[LoginEvent] should call errorHandler if LoginUseCase throws", () => {
      const store = TestBed.inject(TokenStore);

      loginUseCase.call.mockReturnValue(throwError(() => error));

      dispatch.dispatch(tokenLoginEvents.login({ username, password }));

      expect(store.status()).toBe(TokenStatus.failure);
      expect(errorHandler.handleError.mock.calls.length).toBe(1);
      expect(errorHandler.handleError).toHaveBeenCalledWith(error);
    });
  }

  it("[LogoutEvent] should call LogoutUseCase", () => {
    logoutUseCase.call.mockReturnValue(of(void 0));

    dispatch.dispatch(globalEvents.logout());

    expect(logoutUseCase.call.mock.calls.length).toBe(1);
    expect(logoutUseCase.call).toHaveBeenCalledWith();
  });

  for (const error of errorTestData) {
    it("[LogoutEvent] should call errorHandler if LogoutUseCase throws", () => {
      const store = TestBed.inject(TokenStore);

      logoutUseCase.call.mockReturnValue(throwError(() => error));

      dispatch.dispatch(globalEvents.logout());

      expect(store.status()).toBe(TokenStatus.failure);
      expect(errorHandler.handleError.mock.calls.length).toBe(1);
      expect(errorHandler.handleError).toHaveBeenCalledWith(error);
    });
  }

  it("[LogoutEvent] should unset Token if LogoutUseCase returns success", () => {
    const store = TestBed.inject(TokenStore);
    logoutUseCase.call.mockReturnValue(of(void 0));

    store.setToken(token);

    dispatch.dispatch(globalEvents.logout());

    expect(store.token()).toBe("");
    expect(store.status()).toBe(TokenStatus.initial);
  });
});
