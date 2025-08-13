import { Routes } from "@angular/router";
import { Login } from "./features/authentication/components/login/login";
import { Register } from "./features/authentication/components/register/register";

export const routes: Routes = [
  {
    path: "",
    redirectTo: "login",
    pathMatch: "full",
  },
  {
    path: "login",
    component: Login,
  },
  {
    path: "register",
    component: Register,
  },
];
