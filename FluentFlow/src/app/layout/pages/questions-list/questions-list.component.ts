import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../../additions/navbar/navbar.component";
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionsService } from '../../../services/Questions/questions.service';

@Component({
  selector: 'app-questions-list',
  standalone: true,
  imports: [NavbarComponent],
  templateUrl: './questions-list.component.html',
  styleUrl: './questions-list.component.scss'
})
export class QuestionsListComponent implements OnInit
{
  id: string | null = null;
  questions: any[] = [];
  clickedType:number = 0;
  constructor(private route: ActivatedRoute, private _QuestionsService:QuestionsService,private _Router:Router)
  {
    
  }
  ngOnInit(): void 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    
    if(this.id == "1")
    {
      if(this._QuestionsService.readingQuestions.getValue() !== null)
      {
          this.questions = this._QuestionsService.readingQuestions.getValue().reading;
          return;
      }
      this._QuestionsService.getReadingQuestions().subscribe((data) => {
        this.questions = data.reading;
      });
    }
    else if(this.id == "2")
    {
      if(this._QuestionsService.listeningQuestions.getValue() !== null)
      {
          this.questions = this._QuestionsService.listeningQuestions.getValue().listening;
          return;
      }
      this._QuestionsService.getListeningQuestions().subscribe((data) => {
        this.questions = data.listening;
      });
    }
    else if(this.id == "3")
    {
      if(this._QuestionsService.grammarQuestions.getValue() !== null)
      {
        this.questions = this._QuestionsService.grammarQuestions.getValue().grammar;
        return;
      }
      this._QuestionsService.getGrammarQuestions().subscribe((data) => {
        this.questions = data.grammar;
      });
    }
  }

  getIndex(index: number): void
  {
    this.clickedType = index;
    if(this.id == "1")
    {
      this._Router.navigate(['/reading-questions', index]);
    }
    else if(this.id == "2")
    {
      this._Router.navigate(['/listening-questions', index]);
    }
    else if(this.id == "3")
    {
      this._Router.navigate(['/grammar-questions', index]);
    }

  }
  
}
