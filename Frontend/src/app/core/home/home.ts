import { Component, ChangeDetectionStrategy } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";
import { Button } from "../components/button/button";
import { ButtonType } from "../components/button/button-type";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage, Button],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {
  get ButtonType(): typeof ButtonType {
    return ButtonType;
  }
}
