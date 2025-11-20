import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { LoginUseCase } from "./login-usecase";
import { TestBed } from "@angular/core/testing";
import { provideZonelessChangeDetection } from "@angular/core";
import { HttpErrorResponse, provideHttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, of } from "rxjs";
import { testTokenDtos } from "../../../../shared/test-data/dtos/token-dto-test-data";
import { LoginError } from "./login-error";
import { testHttpErrorResponses } from "../../../../shared/test-data/http/http-error-response-test-data";

describe("LoginUseCase", () => {
  // data
  const username = "username";
  const password = "password";
  const loginUrl = "auth:/login";

  // dependencies
  let httpTesting: HttpTestingController;

  // under test
  let loginUseCase: LoginUseCase;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideZonelessChangeDetection(), provideHttpClient(), provideHttpClientTesting()],
    });

    httpTesting = TestBed.inject(HttpTestingController);
    loginUseCase = TestBed.inject(LoginUseCase);
  });

  it("should call the expected url", () => {
    firstValueFrom(loginUseCase.call(username, password));

    httpTesting.expectOne(loginUrl);
    httpTesting.verify();

    expect().nothing();
  });

  for (const tokenDto of testTokenDtos) {
    it("should return the correct token if the request succeeds", async () => {
      const usecase = firstValueFrom(loginUseCase.call(username, password));

      const req = httpTesting.expectOne(loginUrl);
      req.flush(tokenDto);

      const result = await usecase;
      const token = result.isOk() ? result.value : null;

      expect(token).toBe(tokenDto.accessToken);
    });
  }

  it("should return InvalidCredentials if the request returns Unauthorized", async () => {
    const usecase = firstValueFrom(loginUseCase.call(username, password));

    const req = httpTesting.expectOne(loginUrl);
    req.flush("", { status: 401, statusText: "Unauthorized" });

    const result = await usecase;
    const error = result.isErr() ? result.error : null;

    expect(error).toBe(LoginError.InvalidCredentials);
  });

  for (const { status, statusText } of testHttpErrorResponses.filter((r) => r.status != 401)) {
    it(`should throw error if the request returns ${status}:${statusText}`, () => {
      let capturedError: HttpErrorResponse;

      firstValueFrom(
        loginUseCase.call(username, password).pipe(
          catchError((error) => {
            capturedError = error;
            return of(error);
          }),
        ),
      );

      const req = httpTesting.expectOne(loginUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(capturedError!.status).toBe(status);
      expect(capturedError!.statusText).toBe(statusText);
    });
  }
});
