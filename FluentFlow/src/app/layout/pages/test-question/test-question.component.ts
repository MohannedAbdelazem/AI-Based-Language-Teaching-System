import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { log } from 'console';

@Component({
  selector: 'app-test-question',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './test-question.component.html',
  styleUrl: './test-question.component.scss'
})
export class TestQuestionComponent  {


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
