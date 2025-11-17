import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { RefreshUseCase } from "./refresh-usecase";
import { TestBed } from "@angular/core/testing";
import { provideZonelessChangeDetection } from "@angular/core";
import { HttpErrorResponse, provideHttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, of } from "rxjs";
import { testTokenDtos } from "../../../../shared/test-data/dtos/token-dto-test-data";
import { RefreshError } from "./refresh-error";
import { testHttpErrorResponses } from "../../../../shared/test-data/http/http-error-response-test-data";

describe("RefreshUseCase", () => {
  // data
  const refreshUrl = "auth:/refresh";

  // dependencies
  let httpTesting: HttpTestingController;

  // under test
  let refreshUseCase: RefreshUseCase;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideZonelessChangeDetection(), provideHttpClient(), provideHttpClientTesting()],
    });

    httpTesting = TestBed.inject(HttpTestingController);
    refreshUseCase = TestBed.inject(RefreshUseCase);
  });

  it("should call the expected url", () => {
    firstValueFrom(refreshUseCase.call());

    httpTesting.expectOne(refreshUrl);
    httpTesting.verify();

    expect().nothing();
  });

  for (const tokenDto of testTokenDtos) {
    it("should return the correct token if the request succeeds", async () => {
      const usecase = firstValueFrom(refreshUseCase.call());

      const req = httpTesting.expectOne(refreshUrl);
      req.flush(tokenDto);

      const result = await usecase;
      const token = result.isOk() ? result.value : null;

      expect(token).toBe(tokenDto.accessToken);
    });
  }

  it("should return InvalidRefreshToken if the request returns Unauthorized", async () => {
    const usecase = firstValueFrom(refreshUseCase.call());

    const req = httpTesting.expectOne(refreshUrl);
    req.flush("", { status: 401, statusText: "Unauthorized" });

    const result = await usecase;
    const error = result.isErr() ? result.error : null;

    expect(error).toBe(RefreshError.InvalidRefreshToken);
  });

  for (const { status, statusText } of testHttpErrorResponses.filter((r) => r.status != 401)) {
    it(`should throw error if the request returns ${status}:${statusText}`, () => {
      let capturedError: HttpErrorResponse;

      firstValueFrom(
        refreshUseCase.call().pipe(
          catchError((error) => {
            capturedError = error;
            return of(error);
          }),
        ),
      );

      const req = httpTesting.expectOne(refreshUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(capturedError!.status).toBe(status);
      expect(capturedError!.statusText).toBe(statusText);
    });
  }
});
