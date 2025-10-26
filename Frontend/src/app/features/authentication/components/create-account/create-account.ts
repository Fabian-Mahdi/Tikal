import { Component, ChangeDetectionStrategy, inject } from "@angular/core";
import { Menu } from "../../../../core/menu/menu";
import { FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Button } from "../../../../core/components/button/button";
import { ButtonStyle } from "../../../../core/components/button/button-type";
import { AccountCreationStatus, ActiveAccountStore } from "../../stores/active-account/active-account-store";
import { injectDispatch } from "@ngrx/signals/events";
import { activeAccountCreateEvents } from "../../stores/active-account/events/active-account-create-events";
import { LoadingOverlay } from "../../../../core/loading-overlay/loading-overlay";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-create-account",
  imports: [ReactiveFormsModule, Menu, Button, LoadingOverlay],
  templateUrl: "./create-account.html",
  styleUrl: "./create-account.scss",
})
export class CreateAccount {
  readonly ButtonStyle = ButtonStyle;

  readonly AccountCreationStatus = AccountCreationStatus;

  readonly activeAccountStore = inject(ActiveAccountStore);

  private readonly dispatch = injectDispatch(activeAccountCreateEvents);

  private readonly formBuilder: NonNullableFormBuilder = inject(NonNullableFormBuilder);

  readonly accountForm: FormGroup = this.formBuilder.group({
    name: ["", [Validators.required]],
  });

  async onSubmit(): Promise<void> {
    if (this.accountForm.invalid) {
      this.accountForm.markAllAsTouched();
      return;
    }

    const { name }: { name: string } = this.accountForm.value;

    this.dispatch.createAccount(name);
  }

  onCancelPressed(): void {
    this.dispatch.cancel();
  }
}
