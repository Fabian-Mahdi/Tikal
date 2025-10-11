import { HttpInterceptorFn } from "@angular/common/http";
import { timeout } from "rxjs";

export const timeoutInterceptor: HttpInterceptorFn = (req, next) => {
  const defaultTimeout = 10000;

  return next(req).pipe(timeout(defaultTimeout));
};
