import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { backgroundFadeOut } from "../../../../core/menu/animations/fade-out";
import { Menu } from "../../../../core/menu/menu";
import { Router } from "@angular/router";
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { LoadingOverlayService } from "../../../../core/loading-overlay/loading-overlay-service";
import { CreateAccountUseCase } from "../../usecases/create-account/create-account-usecase";
import { ErrorOverlayService } from "../../../../core/error-overlay/error-overlay-service";

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

  private readonly createAccount: CreateAccountUseCase = inject(CreateAccountUseCase);

  private readonly loadingOverlay: LoadingOverlayService = inject(LoadingOverlayService);

  private readonly errorOverlay: ErrorOverlayService = inject(ErrorOverlayService);

  private readonly formBuilder: NonNullableFormBuilder = inject(NonNullableFormBuilder);

  readonly accountForm: FormGroup = this.formBuilder.group({
    name: ["", [Validators.required]],
  });

  async onSubmit(): Promise<void> {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }

    this.loadingOverlay.showLoadingOverlay();

    const { name }: { name: string } = this.accountForm.value;

    const accountCreationResult = await this.createAccount.call(name);

    if (accountCreationResult.isErr()) {
      this.loadingOverlay.hideLoadingOverlay();
      this.router.navigate([{ outlets: { overlay: null } }]);
      this.errorOverlay.showErrorOverlay("An account for your user already exists");
      return;
    }

    this.loadingOverlay.hideLoadingOverlay();
    this.router.navigate([{ outlets: { overlay: null } }]);
  }

  onCancelPressed(): void {
    this.router.navigate([{ outlets: { overlay: null } }]);
  }

  get ButtonStyle(): typeof ButtonStyle {
    return ButtonStyle;
  }
}
