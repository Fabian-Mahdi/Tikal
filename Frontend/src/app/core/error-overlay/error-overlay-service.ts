import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class ErrorOverlayService {
  private readonly errorOverlaySubject = new BehaviorSubject<boolean>(false);

  readonly errorOverlay: Observable<boolean> =
    this.errorOverlaySubject.asObservable();

  private readonly errorMessageSubject = new BehaviorSubject<string>("");

  readonly errorMessage: Observable<string> =
    this.errorMessageSubject.asObservable();

  showErrorOverlay(errorMessage: string): void {
    this.errorMessageSubject.next(errorMessage);
    this.errorOverlaySubject.next(true);
  }

  hideErrorOverlay(): void {
    this.errorOverlaySubject.next(false);
  }
}
