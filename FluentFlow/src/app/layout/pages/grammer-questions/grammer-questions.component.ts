import { Component, OnInit } from '@angular/core';
import { QuestionsService } from '../../../services/Questions/questions.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { GrammarData } from '../../../interfaces/Grammer/grammer';

@Component({
  selector: 'app-grammer-questions',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './grammer-questions.component.html',
  styleUrl: './grammer-questions.component.scss'
})
export class GrammerQuestionsComponent implements OnInit
{
    id: string | null = null;
    questions: GrammarData[]  = [] ;
    questionNumber: number = 0;
    grammar: string = "grammar";
  constructor(private _Router: Router, private _QuestionsService:QuestionsService, private route:ActivatedRoute) { }

  ngOnInit(): void 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id)
    {
      this.questionNumber = parseInt(this.id, 10);
    }
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
