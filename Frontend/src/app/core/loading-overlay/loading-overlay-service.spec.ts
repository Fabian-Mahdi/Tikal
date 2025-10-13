import { TestBed } from "@angular/core/testing";

import { LoadingOverlayService } from "./loading-overlay-service";
import { provideZonelessChangeDetection } from "@angular/core";

describe("LoadingOverlayService", () => {
  let service: LoadingOverlayService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideZonelessChangeDetection()],
    });
    service = TestBed.inject(LoadingOverlayService);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
