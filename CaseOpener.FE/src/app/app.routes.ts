import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { MainComponent } from './main/main.component';
import { AuthGuard } from './guards/auth.guard';
import { ErrorComponent } from './error/error.component';

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
    { path: 'inventory', loadComponent: () =>
        import('./inventory/inventory.component').then(
        (c) => c.InventoryComponent), canActivate: [AuthGuard],
    },
    {
        path: 'case',
        children: [
          {
            path: ':caseId',
            loadComponent: () =>
                import('./case/case-details/case-details.component').then((c) => c.CaseDetailsComponent)
          },
        ],
    },
    { path: 'mycases', loadComponent: () =>
        import('./case/bought-cases/bought-cases.component').then(
        (c) => c.BoughtCasesComponent), canActivate: [AuthGuard],
    }, 
    { path: '404', component: ErrorComponent },
    { path: '**', redirectTo: '/404' },
];
