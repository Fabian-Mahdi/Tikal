import { Component, ChangeDetectionStrategy, inject, signal, WritableSignal } from "@angular/core";
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LoginUseCase } from "../../usecases/login/login-usecase";
import { Menu } from "../../../../core/menu/menu";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { GetCurrentAccountUseCase } from "../../usecases/set-current-account/get-current-account-usecase";
import { LoadingOverlayService } from "../../../../core/loading-overlay/loading-overlay-service";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [ReactiveFormsModule, Menu, Button],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
})
export class Login {
  private readonly router: Router = inject(Router);

  private readonly login: LoginUseCase = inject(LoginUseCase);

  private readonly setAccount: GetCurrentAccountUseCase = inject(GetCurrentAccountUseCase);

  private readonly loadingOverlay: LoadingOverlayService = inject(LoadingOverlayService);

  private readonly formBuilder: NonNullableFormBuilder = inject(NonNullableFormBuilder);

  readonly loginForm: FormGroup = this.formBuilder.group({
    username: ["", [Validators.required]],
    password: ["", [Validators.required]],
  });

  readonly formValid: WritableSignal<boolean> = signal(this.loginForm.valid);

  readonly errorMessage: WritableSignal<string> = signal("");

  constructor() {
    this.loginForm.statusChanges.subscribe(() => {
      this.formValid.set(this.loginForm.valid);
    });
  }

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loadingOverlay.showLoadingOverlay();

    const { username, password }: { username: string; password: string } = this.loginForm.value;

    const loginResult = await this.login.call(username, password);

    if (loginResult.isErr()) {
      this.loadingOverlay.hideLoadingOverlay();
      this.password.setValue("");
      this.errorMessage.set("Invalid username or password provided");
      return;
    }
  }

  onClosePressed(): void {
    this.router.navigate([{ outlets: { overlay: null } }]);
  }

  get password(): FormControl<string> {
    return this.loginForm.get("password") as FormControl<string>;
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
