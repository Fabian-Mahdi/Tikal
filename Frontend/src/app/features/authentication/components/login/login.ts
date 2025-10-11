import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Router } from "@angular/router";
import { LoginUseCase } from "../../usecases/login/login-usecase";
import { Menu } from "../../../../core/menu/menu";
import { backgroundFadeOut } from "../../../../core/menu/animations/fade-out";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [ReactiveFormsModule, Menu, Button],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
  animations: [backgroundFadeOut],
})
export class Login {
  private readonly router: Router = inject(Router);

  private readonly formBuilder: NonNullableFormBuilder = inject(
    NonNullableFormBuilder,
  );

  private readonly login: LoginUseCase = inject(LoginUseCase);

  readonly loginForm: FormGroup = this.formBuilder.group({
    username: ["", [Validators.required]],
    password: ["", [Validators.required]],
  });

  async onSubmit(): Promise<void> {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const { username, password }: { username: string; password: string } =
      this.loginForm.value;

    const result = await this.login.call(username, password);

    if (result.isOk()) {
      console.log("success");
    } else {
      console.log("failure");
    }
  }

  onClosePressed(): void {
    this.router.navigate([{ outlets: { overlay: null } }]);
  }

  get username(): FormControl<string> {
    return this.loginForm.get("username") as FormControl<string>;
  }

  get password(): FormControl<string> {
    return this.loginForm.get("password") as FormControl<string>;
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
