import { ErrorHandler } from "@angular/core";
import { TimeoutError } from "rxjs";

export class DevelopmentErrorHandler implements ErrorHandler {
  handleError(error: unknown): void {
    if (error instanceof TimeoutError) {
      console.log("timeout occured");
    }
  }
}
