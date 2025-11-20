import { ComponentFixture, TestBed } from "@angular/core/testing";
import { Button } from "./button";
import { provideZonelessChangeDetection } from "@angular/core";
import { ButtonStyle } from "./button-type";
import { By } from "@angular/platform-browser";

describe("Button", () => {
  let component: Button;
  let fixture: ComponentFixture<Button>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Button],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(Button);
    fixture.componentRef.setInput("style", ButtonStyle.Primary);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should emit clicked event when clicked", () => {
    let emitted = false;
    component.clicked.subscribe(() => (emitted = true));

    const buttonElement = fixture.debugElement.query(By.css("button"));

    buttonElement.triggerEventHandler("click");

    expect(emitted).toBeTrue();
  });
});
