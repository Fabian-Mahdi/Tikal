import { TestBed } from "@angular/core/testing";
import { TokenStore } from "../../../features/authentication/stores/token/token-store";
import { signal } from "@angular/core";
import { HttpClient, provideHttpClient, withInterceptors } from "@angular/common/http";
import { authenticationInterceptor } from "./authentication-interceptor";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { firstValueFrom } from "rxjs";
import { beforeEach, describe, expect, it } from "vitest";

class MockTokenStore {
  readonly token = signal("");
}

describe("authenticationInterceptor", () => {
  // data
  const testUrl = "testing";

  // dependencies
  let tokenStore: MockTokenStore;

  // under test
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    tokenStore = new MockTokenStore();

    TestBed.configureTestingModule({
      providers: [
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
