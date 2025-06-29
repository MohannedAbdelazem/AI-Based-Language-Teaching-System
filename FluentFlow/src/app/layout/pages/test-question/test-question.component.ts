import { QuestionsService } from './../../../services/Questions/questions.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { log } from 'console';
import { GeneralQuestion } from '../../../interfaces/generalQuestion/general-question';

@Component({
  selector: 'app-test-question',
  standalone: true,
  imports: [],
  templateUrl: './test-question.component.html',
  styleUrl: './test-question.component.scss'
})
export class TestQuestionComponent implements OnInit 
{

  id: string | null = null;
  questionNumber: number = 0;
  questions: GeneralQuestion[] = [];
  currentQuestion: GeneralQuestion | null = null;
  correctAnswer: string = '';
  constructor(private route: ActivatedRoute, private _QuestionsService:QuestionsService, private _Router:Router) {}

  ngOnInit(): void 
  {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) 
    {
      this.questionNumber = parseInt(this.id, 10);
    }

    if(this._QuestionsService.questions.getValue() !== null)
    {
      const data = this._QuestionsService.questions.getValue();
      if (data.reading) 
      {
        this.questions = data.reading[this.questionNumber].questions;
        this.currentQuestion = this.questions[0];
      } 
      else if (data.listening) 
      {
        console.log('Listening data found:', data.listening);
        this.questions = data.listening[this.questionNumber].questions;
        this.currentQuestion = this.questions[0];
      } 
      else if (data.grammar) 
      {
        this.questions = data.grammar[this.questionNumber].questions;
        this.currentQuestion = this.questions[0];
      }
    }
  }

  checkAnswer(answer: string): boolean {
    const answerContainer = document.querySelector('.answer-container div') as HTMLElement;

    if (this.currentQuestion) {
      const isCorrect = this.currentQuestion.rightAnswer === answer;

      if (answerContainer) {

        answerContainer.classList.remove('answer-checking-correct', 'answer-checking-wrong');

        answerContainer.classList.add(isCorrect ? 'answer-checking-correct' : 'answer-checking-wrong');

        answerContainer.innerText = isCorrect ? "That's correct" : "That's wrong";

        answerContainer.style.transform = 'scale(1)';
        answerContainer.style.opacity = '1';

        setTimeout(() => {
          answerContainer.style.transform = 'scale(0.2)';
          answerContainer.style.opacity = '0';
        }, 1000);
      }

      return isCorrect;
    }

    return false;
  }

  submitAnswer(): void {
    const selectedAnswer = document.querySelector('.answerClicked') as HTMLElement;

    if (selectedAnswer) {
      const isCorrect = this.checkAnswer(this.correctAnswer);

      selectedAnswer.classList.remove('answer-correct', 'answer-wrong');

      selectedAnswer.classList.add(isCorrect ? 'answer-correct' : 'answer-wrong');

      setTimeout(() => {
        selectedAnswer.classList.remove('answer-correct', 'answer-wrong');
        if(isCorrect)
        {
          this.getNextQuestion();
        }
      }, 1000);
    }
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
      if(this._QuestionsService.questions.getValue() !== null)
      {
        const data = this._QuestionsService.questions.getValue();
        if (data.reading) 
        {
          this._Router.navigate(['/reading-questions']);
        } 
        else if (data.listening) 
        {
          this._Router.navigate(['/listening-questions']);
        } 
        else if (data.grammar) 
        {
          this._Router.navigate(['/grammar-questions']);
        }
      }
    }
  }


  currentClickedFlag!: HTMLElement;
  changeStyle(event: MouseEvent)
  {
    
    let mytarget: HTMLElement;
    if ((event.target as HTMLElement).tagName === 'H1') 
    {
      this.correctAnswer = (event.target as HTMLElement).innerText;
      mytarget = (event.target as HTMLElement).parentElement as HTMLElement;
    }
    else
    {
      this.correctAnswer = (event.target as HTMLElement).firstChild?.textContent || '';
      mytarget = event.target as HTMLElement;
    }

    
    if (this.currentClickedFlag) {
      this.currentClickedFlag.classList.remove('answerClicked');
    }

    mytarget.classList.add('answerClicked');
    this.currentClickedFlag = mytarget;

  }

}
