import { HttpInterceptorFn } from "@angular/common/http";
import { LoadingService } from "./loading.service";
import { finalize } from "rxjs";

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  LoadingService.isLoading.next(true);

  return next(req).pipe(
    finalize(() => { LoadingService.isLoading.next(false) })
  );
}
