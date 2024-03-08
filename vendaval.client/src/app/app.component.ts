import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LoadingService } from './shared/common/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
//TODO: ADD A GLOBAL ERROR HANDLER
export class AppComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private loadingService: LoadingService) {}

  ngOnInit() {
    this.loadingService.isLoading.subscribe((value) => {
      this.isLoading = value;
    });
  }

  title = 'Vendaval';
  date = new Date();
}
