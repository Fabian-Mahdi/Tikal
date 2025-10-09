import { HttpInterceptorFn } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

export const baseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const [apiPrefix, path] = req.url.split(":/");

  const api = (environment.apis as Record<string, string>)[apiPrefix];

  if (api) {
    const newReq = req.clone({
      url: `${api}/${path}`,
    });

    return next(newReq);
  }

  // default to the main backend
  const newReq = req.clone({
    url: `${environment.apis.main}/${path}`,
  });

  return next(newReq);
};
