import { ErrorHandler, provideZonelessChangeDetection } from "@angular/core";
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

describe("TokenStore", () => {
  // data
  const username = "username";
  const password = "password";
  const token =
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";

  // dependencies
  let errorHandler: jasmine.SpyObj<ErrorHandler>;
  let loginUseCase: jasmine.SpyObj<LoginUseCase>;
  let logoutUseCase: jasmine.SpyObj<LogoutUseCase>;

  // under test
  let dispatch: Dispatcher;

  beforeEach(() => {
    errorHandler = jasmine.createSpyObj("ErrorHandler", ["handleError"]);
    loginUseCase = jasmine.createSpyObj("LoginUseCase", ["call"]);
    logoutUseCase = jasmine.createSpyObj("LogoutUseCase", ["call"]);

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
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
    loginUseCase.call.withArgs(username, password).and.returnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(loginUseCase.call.calls.count()).toBe(1);
  });

  it("[LoginEvent] should emit loadAccount event if LoginUseCase returns success", () => {
    let loadAccountEmitted = false;

    const event = TestBed.inject(Events);
    event
      .on(activeAccountHomeEvents.loadAccount)
      .pipe(tap(() => (loadAccountEmitted = true)))
      .subscribe();

    loginUseCase.call.withArgs(username, password).and.returnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(loadAccountEmitted).toBeTrue();
  });

  it("[LoginEvent] should set token if LoginUseCase returns success", () => {
    const store = TestBed.inject(TokenStore);

    loginUseCase.call.withArgs(username, password).and.returnValue(of(ok(token)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(store.token()).toBe(token);
    expect(store.status()).toBe(TokenStatus.success);
  });

  it("[LoginEvent] should set TokenError if LoginUseCase returns InvalidCredentials", () => {
    const store = TestBed.inject(TokenStore);

    loginUseCase.call.withArgs(username, password).and.returnValue(of(err(LoginError.InvalidCredentials)));

    dispatch.dispatch(tokenLoginEvents.login({ username, password }));

    expect(store.token()).toBe("");
    expect(store.status()).toBe(TokenStatus.unauthorized);
  });

  for (const error of errorTestData) {
    it("[LoginEvent] should call errorHandler if LoginUseCase throws", () => {
      const store = TestBed.inject(TokenStore);

      loginUseCase.call.withArgs(username, password).and.returnValue(throwError(() => error));
      errorHandler.handleError.withArgs(error);

      dispatch.dispatch(tokenLoginEvents.login({ username, password }));

      expect(store.status()).toBe(TokenStatus.failure);
      expect(errorHandler.handleError.calls.count()).toBe(1);
    });
  }

  it("[LogoutEvent] should call LogoutUseCase", () => {
    logoutUseCase.call.and.returnValue(of(void 0));

    dispatch.dispatch(globalEvents.logout());

    expect(logoutUseCase.call.calls.count()).toBe(1);
  });

  it("[LogoutEvent]");

  for (const error of errorTestData) {
    it("[LogoutEvent] should call errorHandler if LogoutUseCase throws", () => {
      const store = TestBed.inject(TokenStore);

      logoutUseCase.call.and.returnValue(throwError(() => error));
      errorHandler.handleError.withArgs(error);

      dispatch.dispatch(globalEvents.logout());

      expect(store.status()).toBe(TokenStatus.failure);
      expect(errorHandler.handleError.calls.count()).toBe(1);
    });
  }

  it("[LogoutEvent] should unset Token if LogoutUseCase returns success", () => {
    const store = TestBed.inject(TokenStore);
    logoutUseCase.call.and.returnValue(of(void 0));

    store.setToken(token);

    dispatch.dispatch(globalEvents.logout());

    expect(store.token()).toBe("");
    expect(store.status()).toBe(TokenStatus.initial);
  });
});
