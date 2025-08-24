import { ComponentFixture, TestBed } from "@angular/core/testing";

import { Menu } from "./menu";
import { provideZonelessChangeDetection } from "@angular/core";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";

describe("Menu", () => {
  let component: Menu;
  let fixture: ComponentFixture<Menu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Menu, NoopAnimationsModule],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(Menu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
