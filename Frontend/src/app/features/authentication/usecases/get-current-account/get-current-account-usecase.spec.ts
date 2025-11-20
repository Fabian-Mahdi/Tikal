import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { GetCurrentAccountUseCase } from "./get-current-account-usecase";
import { TestBed } from "@angular/core/testing";
import { provideZonelessChangeDetection } from "@angular/core";
import { HttpErrorResponse, provideHttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, of } from "rxjs";
import { testAccountDtos } from "../../../../shared/test-data/dtos/account-dto-test-data";
import { GetCurrentAccountError } from "./get-current-account-errors";

describe("GetCurrentAccountUseCase", () => {
  // data
  const getAccountUrl = "main:/accounts";

  // dependencies
  let httpTesting: HttpTestingController;

  // under test
  let getCurrentAccountUseCase: GetCurrentAccountUseCase;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideZonelessChangeDetection(), provideHttpClient(), provideHttpClientTesting()],
    });

    httpTesting = TestBed.inject(HttpTestingController);
    getCurrentAccountUseCase = TestBed.inject(GetCurrentAccountUseCase);
  });

  it("should call the expected url", () => {
    firstValueFrom(getCurrentAccountUseCase.call());

    httpTesting.expectOne(getAccountUrl);
    httpTesting.verify();

    expect().nothing();
  });

  for (const accountDto of testAccountDtos) {
    it("should return the correct account if the requests succeeds", async () => {
      const usecase = firstValueFrom(getCurrentAccountUseCase.call());

      const req = httpTesting.expectOne(getAccountUrl);
      req.flush(accountDto);

      const result = await usecase;
      const account = result.isOk() ? result.value : null;

      expect(account?.username).toBe(accountDto.name);
    });
  }

  it("should return NoAccount if the request returns Not Found", async () => {
    const usecase = firstValueFrom(getCurrentAccountUseCase.call());

    const req = httpTesting.expectOne(getAccountUrl);
    req.flush("", { status: 404, statusText: "Not Found" });

    const result = await usecase;
    const error = result.isErr() ? result.error : null;

    expect(error).toBe(GetCurrentAccountError.NoAccount);
  });

  for (const { status, statusText } of [
    { status: 0, statusText: "Network Error" },
    { status: 400, statusText: "Bad Request" },
    { status: 403, statusText: "Forbidden" },
    { status: 407, statusText: "Conflict" },
    { status: 500, statusText: "Internal Server Error" },
  ]) {
    it(`should throw error if the request returns ${status}:${statusText}`, () => {
      let capturedError: HttpErrorResponse;

      firstValueFrom(
        getCurrentAccountUseCase.call().pipe(
          catchError((error) => {
            capturedError = error;
            return of(error);
          }),
        ),
      );

      const req = httpTesting.expectOne(getAccountUrl);
      req.flush("", { status: status, statusText: statusText });

      expect(capturedError!.status).toBe(status);
      expect(capturedError!.statusText).toBe(statusText);
    });
  }
});
