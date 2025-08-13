import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { User } from "../../models/user";
import { Router } from "@angular/router";
import { LoginUseCase } from "../../usecases/login/login-usecase";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [ReactiveFormsModule],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
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

    const user: User = { username: username };

    const result = await this.login.call(user, password);

    if (result.isOk()) {
      console.log("success");
    } else {
      console.log("failure");
    }

    this.router.navigate(["register"]);
  }

  get username(): FormControl<string> {
    return this.loginForm.get("username") as FormControl<string>;
  }

  get password(): FormControl<string> {
    return this.loginForm.get("password") as FormControl<string>;
  }
}
