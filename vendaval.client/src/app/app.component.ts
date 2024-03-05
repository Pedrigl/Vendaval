import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
//TODO: ADD A GLOBAL ERROR HANDLER
export class AppComponent implements OnInit {

  constructor() {}

  ngOnInit() {
    
  }

  title = 'Vendaval';
  date = new Date();
}
