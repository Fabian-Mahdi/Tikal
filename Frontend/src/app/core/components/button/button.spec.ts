import { ComponentFixture, TestBed } from "@angular/core/testing";

import { Button } from "./button";
import { provideZonelessChangeDetection } from "@angular/core";
import { ButtonType } from "./button-type";

describe("Button", () => {
  let component: Button;
  let fixture: ComponentFixture<Button>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Button],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(Button);
    fixture.componentRef.setInput("type", ButtonType.Primary);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
