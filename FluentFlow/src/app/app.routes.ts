import { Routes } from '@angular/router';
import { ProfileComponent } from './layout/pages/profile/profile.component';
import { LoginComponent } from './layout/pages/login/login.component';
import { RegisterComponent } from './layout/pages/register/register.component';

export const routes: Routes = [
    { path: "", redirectTo: "register", pathMatch: "full" },
    { path: "profile", component: ProfileComponent },
    { path: "login", component: LoginComponent },
    { path: "register", component: RegisterComponent }
];
