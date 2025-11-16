import {
  Component,
  provideZonelessChangeDetection,
  signal,
  WritableSignal,
  ChangeDetectionStrategy,
} from "@angular/core";
import { TestBed } from "@angular/core/testing";
import { provideRouter } from "@angular/router";
import { isAuthenticated } from "./is-authenticated-guard";
import { RouterTestingHarness } from "@angular/router/testing";
import { ActiveAccountStore } from "../../../features/authentication/stores/active-account/active-account-store";

@Component({ changeDetection: ChangeDetectionStrategy.OnPush, template: "<h1>Protected Page</h1>" })
class ProtectedComponent {}

@Component({ changeDetection: ChangeDetectionStrategy.OnPush, template: "<h1>Home</h1>" })
class HomeComponent {}

describe("isAuthenticated guard", () => {
  // data
  const protectedRoute = "protected";

  // dependencies
  let activeAccountStore: { isLoggedIn: WritableSignal<boolean> };

  // under test
  let harness: RouterTestingHarness;

  beforeEach(async () => {
    activeAccountStore = {
      isLoggedIn: signal(true),
    };

    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        {
          provide: ActiveAccountStore,
          useValue: activeAccountStore,
        },
        provideRouter([
          { path: protectedRoute, component: ProtectedComponent, canActivate: [isAuthenticated] },
          { path: "", component: HomeComponent },
        ]),
      ],
    });

    harness = await RouterTestingHarness.create();
  });

  it("should allow navigation when the user is logged in", async () => {
    await harness.navigateByUrl(protectedRoute);

    expect(harness.routeNativeElement?.textContent).toContain("Protected Page");
  });

  it("should redirect to '' when the user is logged out", async () => {
    activeAccountStore.isLoggedIn.set(false);

    await harness.navigateByUrl(protectedRoute);

    expect(harness.routeNativeElement?.textContent).toContain("Home");
  });
});
