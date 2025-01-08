import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';

export const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
];
