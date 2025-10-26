import { ComponentFixture, TestBed } from "@angular/core/testing";

import { LoadingOverlay } from "./loading-overlay";
import { provideZonelessChangeDetection } from "@angular/core";

describe("LoadingOverlay", () => {
  let component: LoadingOverlay;
  let fixture: ComponentFixture<LoadingOverlay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoadingOverlay],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(LoadingOverlay);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
