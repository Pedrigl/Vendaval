import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './shared/common/auth.guard.service';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { AdminComponent } from './admin/admin.component';
import { NotallowedComponent } from './notallowed/notallowed.component';
import { UsersComponent } from './admin/users/users.component';
import { EditUserComponent } from './admin/users/edit-user/edit-user.component';
import { CreateUserComponent } from './admin/users/create-user/create-user.component';
import { ProductComponent } from './admin/products/product.component';
import { CreateProductComponent } from './admin/products/create-product/create-product.component';
import { EditProductComponent } from './admin/products/edit-product/edit-product.component';
import { OrdersComponent } from './admin/orders/orders.component';
import { UserType } from './login/user-type';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {path: 'notallowed', component: NotallowedComponent},
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'orders', component: OrdersComponent, canActivate: [AuthGuardService], data: {roles: [UserType.Admin, UserType.Costumer, UserType.Seller]}},
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin]} },
  {path: 'admin/users', component: UsersComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin]}},
  { path: 'admin/users/edit', component: EditUserComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } },
  { path: 'admin/users/create', component: CreateUserComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } },
  { path: 'admin/products', component: ProductComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } },
  { path: 'admin/products/create', component: CreateProductComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } },
  { path: 'admin/products/edit', component: EditProductComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } },
  { path: 'admin/orders', component: OrdersComponent, canActivate: [AuthGuardService], data: { roles: [UserType.Admin] } }

];

@NgModule({
  imports: [RouterModule.forRoot(routes), FormsModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
