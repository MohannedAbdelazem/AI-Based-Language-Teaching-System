import { Component, ElementRef, HostListener, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { TopicsService } from '../../../services/Topics/topics.service';
import { Topic } from '../../../interfaces/Topics/topics';
import { ProgressComponent } from '../progress/progress.component';
import { NavbarComponent } from '../../additions/navbar/navbar.component';


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ProgressComponent,NavbarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent 
{

  title: string = 'Past Simple';
  titleNumber: number = 1;

  @ViewChildren('lineElement') lineElements!: QueryList<ElementRef>;
  @ViewChild('topicText') topicText!: ElementRef;

  constructor(private __topicService: TopicsService) 
  {
    
  }
  Topics: Topic[] = [];
  ngOnInit() {
    this.__topicService.getTopics().subscribe((data) => {
      this.Topics = data.topics;
    });
  }

  generateRange(n: number): number[] 
  {
  return Array.from({ length: n }, (_, i) => i + 1);
  }

  @HostListener('window:scroll', [])onWindowScroll() 
  {

    this.checkCollision();
  }

  checkCollision() 
  {
    const lines = this.lineElements.toArray();
    const topicRect = this.topicText.nativeElement.getBoundingClientRect();

    for (let i = 0; i < lines.length; i++) 
      {
        const lineRect = lines[i].nativeElement.getBoundingClientRect();

        const isTouching =
          lineRect.bottom >= topicRect.top &&
          lineRect.top <= topicRect.bottom &&
          lineRect.right >= topicRect.left &&
          lineRect.left <= topicRect.right;

        if (isTouching) 
        {
          this.title = lines[i].nativeElement.innerText;
          this.titleNumber = i + 1;
        }
    }
  }

}
