import { Component, OnInit, Renderer2 } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { LoadingService } from './shared/common/loading.service';
import { AuthService } from './shared/common/auth.service';
import { UserType } from './shared/common/enums/user-type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
//TODO: ADD A GLOBAL ERROR HANDLER
export class AppComponent {
  isLoading: boolean = false;
  isChatVisible: boolean = true;
  constructor(private renderer: Renderer2, private authService: AuthService) {
    this.authService.getUser.subscribe((user) => {
      user?.userType != UserType.Seller ? this.isChatVisible = true : this.isChatVisible = false;
    })

    LoadingService.isLoading.subscribe((value) => {
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
