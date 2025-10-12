import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { backgroundFadeOut } from "../../../../core/menu/animations/fade-out";
import { Menu } from "../../../../core/menu/menu";
import { Router } from "@angular/router";
import {
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-create-account",
  imports: [ReactiveFormsModule, Menu, Button],
  templateUrl: "./create-account.html",
  styleUrl: "./create-account.scss",
  animations: [backgroundFadeOut],
})
export class CreateAccount {
  private readonly router: Router = inject(Router);

  private readonly formBuilder: NonNullableFormBuilder = inject(
    NonNullableFormBuilder,
  );

  readonly accountForm: FormGroup = this.formBuilder.group({
    name: ["", [Validators.required]],
  });

  async onSubmit(): Promise<void> {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }
  }

  onCancelPressed(): void {
    this.router.navigate([{ outlets: { overlay: null } }]);
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
