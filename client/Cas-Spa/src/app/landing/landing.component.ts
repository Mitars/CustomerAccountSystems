import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
})
export class LandingComponent implements OnInit {
  username = 'User';
  time = new Date();

  constructor() {}

  ngOnInit() {
    setInterval(() => {
      this.time = new Date();
    }, 1000);
  }
}
