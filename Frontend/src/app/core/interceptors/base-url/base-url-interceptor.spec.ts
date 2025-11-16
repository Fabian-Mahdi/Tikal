import { HttpClient, provideHttpClient, withInterceptors } from "@angular/common/http";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { provideZonelessChangeDetection } from "@angular/core";
import { TestBed } from "@angular/core/testing";
import { baseUrlInterceptor } from "./base-url-interceptor";
import { firstValueFrom } from "rxjs";
import { environment } from "../../../../environments/environment";

describe("baseUrlInterceptor", () => {
  // under test
  let httpClient: HttpClient;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        provideHttpClient(withInterceptors([baseUrlInterceptor])),
        provideHttpClientTesting(),
      ],
    });

    httpClient = TestBed.inject(HttpClient);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  it("should replace the auth prefix with the auth url", () => {
    const authUrl = "auth:/testing";
    const expectedUrl = `${environment.apis.auth}/testing`;

    firstValueFrom(httpClient.get(authUrl));

    const req = httpTesting.expectOne(expectedUrl);

    expect(req.request.url).toBe(expectedUrl);
  });

  it("should replace the main prefix with the main url", () => {
    const mainUrl = "main:/testing";
    const expectedUrl = `${environment.apis.main}/testing`;

    firstValueFrom(httpClient.get(mainUrl));

    const req = httpTesting.expectOne(expectedUrl);

    expect(req.request.url).toBe(expectedUrl);
  });

  it("should replace an unknown prefix with the main url", () => {
    const unknownUrl = "unknown:/testing";
    const expectedUrl = `${environment.apis.main}/testing`;

    firstValueFrom(httpClient.get(unknownUrl));

    const req = httpTesting.expectOne(expectedUrl);

    expect(req.request.url).toBe(expectedUrl);
  });
});
