import { Routes } from "@angular/router";
import { Background } from "./core/components/background/background";
import { Login } from "./features/authentication/components/login/login";
import { CreateAccount } from "./features/authentication/components/create-account/create-account";
import { Lobbies } from "./features/lobbies/components/lobbies/lobbies";
import { Home } from "./core/components/home/home";

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
