import { Component, OnInit, Renderer2 } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LoadingService } from './shared/common/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
//TODO: ADD A GLOBAL ERROR HANDLER
export class AppComponent {
  isLoading: boolean = false;
  constructor(private loadingService: LoadingService, private renderer: Renderer2) {
    this.loadingService.isLoading.subscribe((value) => {
      this.isLoading = value;

      if (this.isLoading) {
        this.renderer.addClass(document.body, 'disable-pointer-events');
      } else {
        this.renderer.removeClass(document.body, 'disable-pointer-events');
      }
    });
  }

  title = 'Vendaval';
  date = new Date();
}
