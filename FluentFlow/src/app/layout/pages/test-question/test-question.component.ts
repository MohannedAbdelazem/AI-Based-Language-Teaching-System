import { QuestionsService } from './../../../services/Questions/questions.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { log } from 'console';
import { GeneralQuestion } from '../../../interfaces/generalQuestion/general-question';

@Component({
  selector: 'app-test-question',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './test-question.component.html',
  styleUrl: './test-question.component.scss'
})
export class TestQuestionComponent implements OnInit 
{

  id: string | null = null;
  questionNumber: number = 0;
  questions: GeneralQuestion[] = [];
  currentQuestion: GeneralQuestion | null = null;
  constructor(private route: ActivatedRoute, private _QuestionsService:QuestionsService, private _Router:Router) {}

  ngOnInit(): void 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) 
    {
      this.questionNumber = parseInt(this.id, 10);
    }
    this._QuestionsService.questions.subscribe((data) => {
      if (data) {
        if (data.reading !== null) {
          this.questions = data.reading[this.questionNumber].questions;
          this.currentQuestion = this.questions[0];
        } else if (data.listening !== null ) {
          this.questions = data.listening[this.questionNumber].questions;
          this.currentQuestion = this.questions[0];
        } else if (data.grammar !== null) {
          this.questions = data.grammar[this.questionNumber].questions;
          this.currentQuestion = this.questions[0];
        }
      }
    });
  }

  getNextQuestion(): void
  {
    if (this.currentClickedFlag) {
      this.currentClickedFlag.classList.remove('answerClicked');
    }
    if (this.questionNumber < this.questions.length - 1) 
    {
      this.questionNumber++;
      this.currentQuestion = this.questions[this.questionNumber];
    }
    else
    {
      this._Router.navigate(['/profile']);
    }
  }

  getPreviousQuestion(): void
  {
    if (this.currentClickedFlag) {
      this.currentClickedFlag.classList.remove('answerClicked');
    }
    if (this.questionNumber > 0) 
    {
      this.questionNumber--;
      this.currentQuestion = this.questions[this.questionNumber];
    }
    else
    {
      this._Router.navigate(['/questions-types']);
    }
  }


  currentClickedFlag!: HTMLElement;
  changeStyle(event: MouseEvent)
  {
    let mytarget: HTMLElement;
    if ((event.target as HTMLElement).tagName === 'H1') 
    {
      mytarget = (event.target as HTMLElement).parentElement as HTMLElement;
    }
    else
    {
      mytarget = event.target as HTMLElement;
    }

    
    if (this.currentClickedFlag) {
      this.currentClickedFlag.classList.remove('answerClicked');
    }

    mytarget.classList.add('answerClicked');
    this.currentClickedFlag = mytarget;

  }

}
