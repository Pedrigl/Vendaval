import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { NotallowedComponent } from './notallowed/notallowed.component';
import { UsersComponent } from './admin/users/users.component';
import { EditUserComponent } from './admin/users/edit-user/edit-user.component';
import { CreateUserComponent } from './admin/users/create-user/create-user.component';
import { ProductComponent } from './admin/products/product.component';
import { CreateProductComponent } from './admin/products/create-product/create-product.component';
import { EditProductComponent } from './admin/products/edit-product/edit-product.component';
import { OrdersComponent } from './admin/orders/orders.component';
import { OrderComponent } from './order/order.component';
import {ProductsComponent } from './product/products.component';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ChatSellerComponent } from './chat/chat-seller/chat-seller.component'; 
import { httpInterceptor } from './shared/common/httpInterceptor';
import { ChatCostumerComponent } from './chat/chat-costumer/chat-costumer.component';
@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    LoginComponent,
    HomeComponent,
    AdminComponent,
    NotallowedComponent,
    UsersComponent,
    EditUserComponent,
    CreateUserComponent,
    ProductComponent,
    CreateProductComponent,
    EditProductComponent,
    OrdersComponent,
    OrderComponent,
    ProductsComponent,
    ChatSellerComponent,
    ChatCostumerComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    NgbModule, CommonModule
  ],
  providers: [provideHttpClient(withInterceptors([httpInterceptor]))],
  bootstrap: [AppComponent]
})
export class AppModule { }
