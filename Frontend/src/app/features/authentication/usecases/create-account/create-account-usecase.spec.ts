import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { CreateAccountUseCase } from "./create-account-usecase";
import { TestBed } from "@angular/core/testing";
import { HttpErrorResponse, provideHttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, of } from "rxjs";
import { testAccountDtos } from "../../../../shared/test-data/dtos/account-dto-test-data";
import { CreateAccountError } from "./create-account-errors";
import { beforeEach, describe, expect, it } from "vitest";
import { testHttpErrorResponses } from "../../../../shared/test-data/http/http-error-response-test-data";

describe("CreateAccountUseCase", () => {
  // data
  const accountName = "name";
  const createAccountUrl = "main:/accounts";

  // dependencies
  let httpTesting: HttpTestingController;

  // under test
  let createAccountUseCase: CreateAccountUseCase;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });

    httpTesting = TestBed.inject(HttpTestingController);
    createAccountUseCase = TestBed.inject(CreateAccountUseCase);
  });

  it("should call the expected url", () => {
    firstValueFrom(createAccountUseCase.call(accountName));

    httpTesting.expectOne(createAccountUrl);
    httpTesting.verify();
  });

  for (const accountDto of testAccountDtos) {
    it("should return the correct account if the requests succeeds", async () => {
      const usecase = firstValueFrom(createAccountUseCase.call(accountDto.name));

      const req = httpTesting.expectOne(createAccountUrl);
      req.flush(accountDto);

      const result = await usecase;
      const account = result.isOk() ? result.value : null;

      expect(account?.username).toBe(accountDto.name);
    });
  }

  it("should return AccountExists if the request returns Conflict", async () => {
    const usecase = firstValueFrom(createAccountUseCase.call(accountName));

    const req = httpTesting.expectOne(createAccountUrl);
    req.flush("", { status: 409, statusText: "Conflict" });

    const result = await usecase;
    const error = result.isErr() ? result.error : null;

    expect(error).toBe(CreateAccountError.AccountExists);
  });

  for (const { status, statusText } of testHttpErrorResponses.filter((r) => r.status != 409)) {
    it(`should throw error if the request returns ${status}:${statusText}`, () => {
      let capturedError: HttpErrorResponse;

      firstValueFrom(
        createAccountUseCase.call(accountName).pipe(
          catchError((error) => {
            capturedError = error;
            return of(error);
          }),
        ),
      );

      const req = httpTesting.expectOne(createAccountUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(capturedError!.status).toBe(status);
    });
  }
});
