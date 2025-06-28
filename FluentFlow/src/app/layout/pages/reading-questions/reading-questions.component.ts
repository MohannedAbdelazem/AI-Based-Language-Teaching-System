import { QuestionsService } from './../../../services/Questions/questions.service';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-reading-questions',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './reading-questions.component.html',
  styleUrl: './reading-questions.component.scss'
})
export class ReadingQuestionsComponent implements OnInit
{
  questions: any[] = [];
  constructor(private _QuestionsService:QuestionsService) { }

  ngOnInit(): void 
  {
    this._QuestionsService.getReadingQuestions().subscribe((data) => {
      this.questions = data;
    });
  }


}
