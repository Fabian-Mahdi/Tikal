import { signal } from "@angular/core";
import { TokenStore } from "../../../features/authentication/stores/token/token-store";
import { HttpClient, HttpErrorResponse, provideHttpClient, withInterceptors } from "@angular/common/http";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { TestBed } from "@angular/core/testing";
import { refreshInterceptor } from "./refresh-interceptor";
import { catchError, firstValueFrom, of } from "rxjs";
import { environment } from "../../../../environments/environment";
import { RefreshUseCase } from "../../../features/authentication/usecases/refresh/refresh-usecase";
import { err, ok } from "neverthrow";
import { RefreshError } from "../../../features/authentication/usecases/refresh/refresh-error";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { testHttpErrorResponses } from "../../../shared/test-data/http/http-error-response-test-data";

class MockRefreshUseCase {
  call = vi.fn();
}

class MockTokenStore {
  readonly token = signal("");

  setToken(token: string): void {
    this.token.set(token);
  }
}

describe("refreshInterceptor", () => {
  // data
  const authUrl = `${environment.apis.auth}/testing`;
  const mainUrl = "main:/testing";

  const successfullRefreshResponse = ok("token");
  const failedRefreshResponse = err(RefreshError.InvalidRefreshToken);

  const unauthorizedResponse = { status: 401, statusText: "Unauthorized" };

  // dependencies
  let tokenStore: MockTokenStore;
  let refreshUseCase: MockRefreshUseCase;

  // under test
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    refreshUseCase = new MockRefreshUseCase();
    refreshUseCase.call.mockReturnValue(of(successfullRefreshResponse));

    tokenStore = new MockTokenStore();

    TestBed.configureTestingModule({
      providers: [
        {
          provide: TokenStore,
          useValue: tokenStore,
        },
        {
          provide: RefreshUseCase,
          useValue: refreshUseCase,
        },
        provideHttpClient(withInterceptors([refreshInterceptor])),
        provideHttpClientTesting(),
      ],
    });

    httpClient = TestBed.inject(HttpClient);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  it("should not refresh when the authentication api returns unauthorized", () => {
    firstValueFrom(httpClient.get(authUrl).pipe(catchError((error) => of(error))));

    const req = httpTesting.expectOne(authUrl);
    req.flush("", unauthorizedResponse);

    expect(vi.mocked(refreshUseCase.call).mock.calls.length).toEqual(0);
  });

  it("should refresh when the main api returns unauthorized", () => {
    firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", unauthorizedResponse);

    expect(vi.mocked(refreshUseCase.call).mock.calls.length).toEqual(1);
  });

  for (const { status, statusText } of testHttpErrorResponses.filter((r) => r.status != 401)) {
    it(`should not refresh when the main api returns ${status}: '${statusText}'`, () => {
      firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

      const req = httpTesting.expectOne(mainUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(vi.mocked(refreshUseCase.call).mock.calls.length).toEqual(0);
    });
  }

  it("should set the token if the refresh is successfull", () => {
    const expectedToken = "new authentication token";
    refreshUseCase.call.mockReturnValue(of(ok(expectedToken)));

    firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", unauthorizedResponse);

    expect(tokenStore.token()).toEqual(expectedToken);
  });

  it("should retry the original request if refresh is successfull", () => {
    firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", unauthorizedResponse);

    httpTesting.expectOne(mainUrl);
  });

  it("should throw the original error if refresh is unsuccessfull", () => {
    let capturedError: HttpErrorResponse;

    refreshUseCase.call.mockReturnValue(of(failedRefreshResponse));

    firstValueFrom(
      httpClient.get(mainUrl).pipe(
        catchError((error) => {
          capturedError = error;
          return of(error);
        }),
      ),
    );

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", { status: 401, statusText: "Unauthorized" });

    httpTesting.expectNone(mainUrl);
    expect(capturedError!.status).toEqual(401);
  });
});
