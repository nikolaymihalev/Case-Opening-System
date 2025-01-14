import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { MainComponent } from './main/main.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'home', component: MainComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'history', loadComponent: () =>
        import('./case/opened-cases/opened-cases.component').then(
        (c) => c.OpenedCasesComponent), canActivate: [AuthGuard],
    },
    { path: 'settings', loadComponent: () =>
        import('./user/profile/profile.component').then(
        (c) => c.ProfileComponent), canActivate: [AuthGuard],
    },
];
