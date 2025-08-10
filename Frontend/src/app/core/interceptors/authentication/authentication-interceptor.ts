import { HttpInterceptorFn } from "@angular/common/http";

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const accessToken = localStorage.getItem("tikalAccessToken");

  const newReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  return next(newReq);
};
