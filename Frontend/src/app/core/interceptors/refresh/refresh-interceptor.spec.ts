import { provideZonelessChangeDetection, signal, WritableSignal } from "@angular/core";
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

describe("refreshInterceptor", () => {
  // data
  const authUrl = `${environment.apis.auth}/testing`;
  const mainUrl = "main:/testing";

  const successfullRefreshResponse = ok("token");
  const failedRefreshResponse = err(RefreshError.InvalidRefreshToken);

  const unauthorizedResponse = { status: 401, statusText: "Unauthorized" };

  // dependencies
  let tokenStore: { token: WritableSignal<string>; setToken: (token: string) => void };
  let refreshSpy: jasmine.SpyObj<RefreshUseCase>;

  // under test
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    refreshSpy = jasmine.createSpyObj("RefreshUseCase", ["call"]);
    refreshSpy.call.and.returnValue(of(successfullRefreshResponse));

    tokenStore = {
      token: signal(""),
      setToken(token: string): void {
        this.token.set(token);
      },
    };

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        {
          provide: TokenStore,
          useValue: tokenStore,
        },
        {
          provide: RefreshUseCase,
          useValue: refreshSpy,
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

    expect(refreshSpy.call.calls.count()).toEqual(0);
  });

  it("should refresh when the main api returns unauthorized", () => {
    firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", unauthorizedResponse);

    expect(refreshSpy.call.calls.count()).toEqual(1);
  });

  for (const { status, statusText } of [
    { status: 403, statusText: "Forbidden" },
    { status: 200, statusText: "Ok" },
    { status: 500, statusText: "Internal Server Error" },
    { status: 400, statusText: "Bad Request" },
  ]) {
    it(`should not refresh when the main api returns ${status}: '${statusText}'`, () => {
      firstValueFrom(httpClient.get(mainUrl).pipe(catchError((error) => of(error))));

      const req = httpTesting.expectOne(mainUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(refreshSpy.call.calls.count()).toEqual(0);
    });
  }

  it("should set the token if the refresh is successfull", () => {
    const expectedToken = "new authentication token";
    refreshSpy.call.and.returnValue(of(ok(expectedToken)));

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

    expect().nothing();
  });

  it("should throw the original error if refresh is unsuccessfull", () => {
    const expectedStatusText = "My very own custom error message";
    let capturedError: HttpErrorResponse;

    refreshSpy.call.and.returnValue(of(failedRefreshResponse));

    firstValueFrom(
      httpClient.get(mainUrl).pipe(
        catchError((error) => {
          capturedError = error;
          return of(error);
        }),
      ),
    );

    const req = httpTesting.expectOne(mainUrl);
    req.flush("", { status: 401, statusText: expectedStatusText });

    httpTesting.expectNone(mainUrl);
    expect(capturedError!.statusText).toEqual(expectedStatusText);
  });
});
