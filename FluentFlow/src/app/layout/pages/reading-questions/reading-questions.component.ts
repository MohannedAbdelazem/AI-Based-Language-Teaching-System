import { QuestionsService } from './../../../services/Questions/questions.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReadingData } from '../../../interfaces/Reading/reading';

@Component({
  selector: 'app-reading-questions',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './reading-questions.component.html',
  styleUrl: './reading-questions.component.scss'
})
export class ReadingQuestionsComponent implements OnInit
{
  id: string | null = null;
  questions: ReadingData[]  = [] ;
  pragraphNumber: number = 0;
  reading: string = 'reading';
  constructor(private _QuestionsService:QuestionsService, private route:ActivatedRoute) { }

  ngOnInit(): void 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    if(this.id === 'flags')
    {
      this._QuestionsService.requieredQuestions.next(2);
      
    }
    else
    {
      if (this.id)
      {
        this.pragraphNumber = parseInt(this.id, 10);
      }
    }

    if(this._QuestionsService.readingQuestions.getValue() !== null)
    {
        this.questions = this._QuestionsService.readingQuestions.getValue().reading;
        return;
    }
    this._QuestionsService.getReadingQuestions().subscribe((data) => {
      this.questions = data.reading;
    });
  }
  
}
