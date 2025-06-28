import { QuestionsService } from './../../../services/Questions/questions.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
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
  constructor(private route: ActivatedRoute, private _QuestionsService:QuestionsService) {}

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
        } else if (data.listening !== null ) {
          this.questions = data.listening[this.questionNumber].questions;
        } else if (data.grammar !== null) {
          this.questions = data.grammar[this.questionNumber].questions;
        }
      }
    });
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
