import { Component, ElementRef, OnInit, QueryList, viewChild, ViewChildren, viewChildren } from '@angular/core';
import { FlagsService } from '../../../services/Flags/flags.service';
import { Flag } from '../../../interfaces/Flags/flag';
import { log } from 'console';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-flags',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './flags.component.html',
  styleUrl: './flags.component.scss'
})
export class FlagsComponent implements OnInit 
{
  counter: number = 1;

  currentClickedFlag!: HTMLImageElement;
  flagsRecived: Flag[] = [];

  question:string = 'What is your native language?';
  constructor(private _FlagsService: FlagsService, private _Router: Router) 
  {
  }
  ngOnInit(): void {
    this.getFlags();
  }

  changeStyle(event: MouseEvent)
  {
    const target = event.target as HTMLImageElement;
    if (this.currentClickedFlag) {
      this.currentClickedFlag.classList.remove('imageclick');
    }

    target.classList.add('imageclick');
    this.currentClickedFlag = target;

  }

  getFlags() 
  {
    this._FlagsService.getFlags().subscribe({
      next: (res) => {
        this.flagsRecived = res.slice(0, 10);
      },  
      error: (err) => {
        console.error('Error fetching flags:', err);
      }
    });
  }

  
  getNextFlag()
  {
    if (this.counter == 1)
    {
      if (this.currentClickedFlag) 
      {
        this.currentClickedFlag.classList.remove('imageclick');
      }
      this.counter++;
      this.question = 'Which language do you want to learn?';
    }
    else
    {
      this._Router.navigate(['/reading-questions','flags']);

    }
  }


}
