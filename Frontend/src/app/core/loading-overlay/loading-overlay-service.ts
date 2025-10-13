import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class LoadingOverlayService {
  private loadingOverlaySubject = new BehaviorSubject<boolean>(false);

  readonly loadingOverlay: Observable<boolean> = this.loadingOverlaySubject.asObservable();

  showLoadingOverlay(): void {
    this.loadingOverlaySubject.next(true);
  }

  hideLoadingOverlay(): void {
    this.loadingOverlaySubject.next(false);
  }
}
