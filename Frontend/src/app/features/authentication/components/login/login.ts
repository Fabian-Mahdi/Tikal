import { Component, ChangeDetectionStrategy, inject, signal, WritableSignal } from "@angular/core";
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Menu } from "../../../../core/components/menu/menu";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { TokenStatus, TokenStore } from "../../stores/token/token-store";
import { tokenLoginEvents } from "../../stores/token/events/token-login-events";
import { injectDispatch } from "@ngrx/signals/events";
import { LoadingOverlay } from "../../../../core/overlays/loading-overlay/loading-overlay";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [ReactiveFormsModule, Menu, Button, LoadingOverlay],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
})
export class Login {
  readonly ButtonStyle = ButtonStyle;

  readonly TokenStatus = TokenStatus;

  readonly tokenStore = inject(TokenStore);

  private readonly dispatch = injectDispatch(tokenLoginEvents);

  private readonly formBuilder = inject(NonNullableFormBuilder);

  readonly loginForm: FormGroup = this.formBuilder.group({
    username: ["", [Validators.required]],
    password: ["", [Validators.required]],
  });

  readonly formValid: WritableSignal<boolean> = signal(this.loginForm.valid);

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

    const { username, password }: { username: string; password: string } = this.loginForm.value;

    this.dispatch.login({ username, password });

    this.password.setValue("");
  }

  onClosePressed(): void {
    this.dispatch.cancel();
  }

  get password(): FormControl<string> {
    return this.loginForm.get("password") as FormControl<string>;
  }
}
