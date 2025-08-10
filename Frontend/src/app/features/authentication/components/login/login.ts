import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { LoginService } from "../../services/login/login-service";
import { User } from "../../models/user";
import { firstValueFrom } from "rxjs";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-login",
  imports: [ReactiveFormsModule],
  templateUrl: "./login.html",
  styleUrl: "./login.scss",
})
export class Login {
  private readonly formBuilder: NonNullableFormBuilder = inject(
    NonNullableFormBuilder,
  );

  private readonly loginService: LoginService = inject(LoginService);

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

    const result = await firstValueFrom(
      this.loginService.login(user, password),
    );

    if (result.isSuccess) {
      console.log("success");
    } else {
      console.log("failure");
    }
  }

  get username(): FormControl<string> {
    return this.loginForm.get("username") as FormControl<string>;
  }

  get password(): FormControl<string> {
    return this.loginForm.get("password") as FormControl<string>;
  }
}
