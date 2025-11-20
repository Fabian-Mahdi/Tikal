import { HttpErrorResponse } from "@angular/common/http";

export const errorTestData: Error[] = [new HttpErrorResponse({ status: 401, statusText: "Unauthorized" })];
