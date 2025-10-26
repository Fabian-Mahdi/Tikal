import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { TokenStore } from "../../../features/authentication/stores/token/token-store";

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenStore = inject(TokenStore);

  const accessToken = tokenStore.token();

  const newReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  return next(newReq);
};
