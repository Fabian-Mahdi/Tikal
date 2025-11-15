import { TestBed } from "@angular/core/testing";
import { TokenStatus, TokenStore } from "../../../features/authentication/stores/token/token-store";
import { provideZonelessChangeDetection, signal } from "@angular/core";
import { HttpClient, provideHttpClient, withInterceptors } from "@angular/common/http";
import { authenticationInterceptor } from "./authentication-interceptor";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { firstValueFrom } from "rxjs";

describe("authenticationInterceptor", () => {
  const testUrl = "testing";

  const tokenStore = {
    token: signal("token"),
    status: signal(TokenStatus.success),
  };

  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
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
