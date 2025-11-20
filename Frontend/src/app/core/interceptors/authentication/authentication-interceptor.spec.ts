import { TestBed } from "@angular/core/testing";
import { TokenStore } from "../../../features/authentication/stores/token/token-store";
import { provideZonelessChangeDetection, signal, WritableSignal } from "@angular/core";
import { HttpClient, provideHttpClient, withInterceptors } from "@angular/common/http";
import { authenticationInterceptor } from "./authentication-interceptor";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { firstValueFrom } from "rxjs";

describe("authenticationInterceptor", () => {
  // data
  const testUrl = "testing";

  // dependencies
  let tokenStore: { token: WritableSignal<string> };

  // under test
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    tokenStore = {
      token: signal("token"),
    };

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        {
          provide: TokenStore,
          useValue: tokenStore,
        },
        provideHttpClient(withInterceptors([authenticationInterceptor])),
        provideHttpClientTesting(),
      ],
    });

    httpClient = TestBed.inject(HttpClient);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  it("should add the stored token as bearer token in the Authorization header", () => {
    firstValueFrom(httpClient.get(testUrl));

    const req = httpTesting.expectOne(testUrl);

    expect(req.request.headers.get("Authorization")).toEqual(`Bearer ${tokenStore.token()}`);
  });
});
