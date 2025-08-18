import { Routes } from "@angular/router";
import { Background } from "./core/background/background";
import { Home } from "./core/home/home";
import { Menu } from "./core/menu/menu";

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
        path: "test",
        component: Menu,
      },
    ],
  },
];
