import { ComponentFixture, TestBed } from "@angular/core/testing";

import { Background } from "./background";
import { provideZonelessChangeDetection } from "@angular/core";

describe("Background", () => {
  let component: Background;
  let fixture: ComponentFixture<Background>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Background],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(Background);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
