import { Routes } from "@angular/router";
import { Background } from "./core/background/background";
import { Home } from "./core/home/home";
import { Login } from "./features/authentication/components/login/login";

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
        path: "login",
        component: Login,
      },
    ],
  },
];
