import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { LogoutUseCase } from "./logout-usecase";
import { TestBed } from "@angular/core/testing";
import { HttpErrorResponse, provideHttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, of } from "rxjs";
import { testHttpErrorResponses } from "../../../../shared/test-data/http/http-error-response-test-data";
import { beforeEach, describe, expect, it } from "vitest";

describe("LogoutUseCase", () => {
  // data
  const logoutUrl = "auth:/logout";

  // dependencies
  let httpTesting: HttpTestingController;

  // under test
  let logoutUseCase: LogoutUseCase;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });

    httpTesting = TestBed.inject(HttpTestingController);
    logoutUseCase = TestBed.inject(LogoutUseCase);
  });

  it("should call the expected url", () => {
    firstValueFrom(logoutUseCase.call());

    const req = httpTesting.expectOne(logoutUrl);
    req.flush("");
    httpTesting.verify();
  });

  for (const { status, statusText } of testHttpErrorResponses) {
    it(`should throw error if the request returns ${status}:${statusText}`, () => {
      let capturedError: HttpErrorResponse;

      firstValueFrom(
        logoutUseCase.call().pipe(
          catchError((error) => {
            capturedError = error;
            return of(error);
          }),
        ),
      );

      const req = httpTesting.expectOne(logoutUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(capturedError!.status).toBe(status);
    });
  }
});
