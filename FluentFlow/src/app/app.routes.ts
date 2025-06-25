import { Routes } from '@angular/router';
import { ProfileComponent } from './layout/pages/profile/profile.component';
import { LoginComponent } from './layout/pages/login/login.component';
import { RegisterComponent } from './layout/pages/register/register.component';
import { QuestionsTypesComponent } from './layout/pages/questions-types/questions-types.component';
import { TestQuestionComponent } from './layout/pages/test-question/test-question.component';
import { FlagsComponent } from './layout/pages/flags/flags.component';

export const routes: Routes = [
    { path: "", redirectTo: "login", pathMatch: "full" },
    { path: "profile", component: ProfileComponent },
    { path: "login", component: LoginComponent },
    { path: "register", component: RegisterComponent },
    { path: "questions-types", component: QuestionsTypesComponent },
    { path: "test-question", component: TestQuestionComponent },
    { path: "flags", component: FlagsComponent }
];
