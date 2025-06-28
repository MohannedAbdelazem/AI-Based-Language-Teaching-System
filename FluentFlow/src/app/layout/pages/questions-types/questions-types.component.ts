import { Component } from '@angular/core';
import { NavbarComponent } from "../../additions/navbar/navbar.component";
import { ProgressComponent } from "../progress/progress.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-questions-types',
  standalone: true,
  imports: [NavbarComponent, ProgressComponent,RouterLink],
  templateUrl: './questions-types.component.html',
  styleUrl: './questions-types.component.scss'
})
export class QuestionsTypesComponent {

}
