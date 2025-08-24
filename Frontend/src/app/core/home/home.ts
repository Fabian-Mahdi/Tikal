import { Component, ChangeDetectionStrategy } from "@angular/core";
import { NgOptimizedImage } from "@angular/common";

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: "app-home",
  imports: [NgOptimizedImage],
  templateUrl: "./home.html",
  styleUrl: "./home.scss",
})
export class Home {}
