import { Routes } from "@angular/router";
import { Background } from "./core/components/background/background";
import { Login } from "./features/authentication/components/login/login";
import { CreateAccount } from "./features/authentication/components/create-account/create-account";
import { Lobbies } from "./features/lobbies/components/lobbies/lobbies";
import { Home } from "./core/components/home/home";
import { isAuthenticated } from "./core/route-guards/is-authenticated/is-authenticated-guard";

export const routes: Routes = [
  {
    path: "",
    component: Background,
    children: [
      {
        path: "",
        component: Home,
      },
      {
        path: "lobbies",
        component: Lobbies,
        canActivate: [isAuthenticated],
      },
    ],
  },
  // full screen overlays
  {
    path: "login",
    component: Login,
    outlet: "overlay",
  },
  {
    path: "createaccount",
    component: CreateAccount,
    outlet: "overlay",
  },
];
