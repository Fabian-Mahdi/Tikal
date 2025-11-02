import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { map, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class LogoutUseCase {
  private readonly httpClient: HttpClient = inject(HttpClient);

  call(): Observable<void> {
    const request = this.httpClient.post("auth:/logout", "").pipe(
      map(() => {
        return;
      }),
    );

    return request;
  }
}

