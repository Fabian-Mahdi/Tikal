import { ComponentFixture, TestBed } from "@angular/core/testing";

import { Lobbies } from "./lobbies";
import { provideZonelessChangeDetection } from "@angular/core";

describe("Lobbies", () => {
  let component: Lobbies;
  let fixture: ComponentFixture<Lobbies>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Lobbies],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents();

    fixture = TestBed.createComponent(Lobbies);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});

