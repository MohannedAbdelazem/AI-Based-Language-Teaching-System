import { Routes } from '@angular/router';
import { ProfileComponent } from './layout/pages/profile/profile.component';
import { LoginComponent } from './layout/pages/login/login.component';
import { RegisterComponent } from './layout/pages/register/register.component';
import { QuestionsTypesComponent } from './layout/pages/questions-types/questions-types.component';
import { TestQuestionComponent } from './layout/pages/test-question/test-question.component';
import { FlagsComponent } from './layout/pages/flags/flags.component';
import { ListeningQuestionsComponent } from './layout/pages/listening-questions/listening-questions.component';
import { ReadingQuestionsComponent } from './layout/pages/reading-questions/reading-questions.component';
import { Component } from '@angular/core';
import { QuestionsListComponent } from './layout/pages/questions-list/questions-list.component';
import { GrammerQuestionsComponent } from './layout/pages/grammer-questions/grammer-questions.component';

export const routes: Routes = [
    { path: "", redirectTo: "login", pathMatch: "full" },
    { path: "profile", component: ProfileComponent },
    { path: "login", component: LoginComponent },
    { path: "register", component: RegisterComponent },
    { path: "questions-types", component: QuestionsTypesComponent },
    { path: "test-question/:id/:pageName", component: TestQuestionComponent },
    { path: "flags", component: FlagsComponent },
    { path: "listening-questions/:id", component: ListeningQuestionsComponent },
    { path: "reading-questions/:id", component: ReadingQuestionsComponent }, 
    { path: "questions-list/:id", component: QuestionsListComponent},
    { path: "grammar-questions/:id", component: GrammerQuestionsComponent }
];
