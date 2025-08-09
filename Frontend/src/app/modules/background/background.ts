import { ChangeDetectionStrategy, Component } from "@angular/core";
import * as THREE from "three";

@Component({
  selector: "app-background",
  imports: [],
  templateUrl: "./background.html",
  styleUrl: "./background.scss",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Background {
  private scene: THREE.Scene;

  constructor() {
    this.scene = new THREE.Scene();
  }
}
