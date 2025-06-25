import { Component, ElementRef, OnInit, QueryList, viewChild, ViewChildren, viewChildren } from '@angular/core';
import { FlagsService } from '../../../services/Flags/flags.service';
import { Flag } from '../../../interfaces/Flags/flag';
import { log } from 'console';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-flags',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './flags.component.html',
  styleUrl: './flags.component.scss'
})
export class FlagsComponent implements OnInit 
{

  // @viewChild

  currentClickedFlag!: HTMLImageElement;
  flagsRecived: Flag[] = [];

  constructor(private _FlagsService: FlagsService) 
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
        console.log('Flags fetched successfully:', this.flagsRecived);
      },  
      error: (err) => {
        console.error('Error fetching flags:', err);
      }
    });
  }

  



}
