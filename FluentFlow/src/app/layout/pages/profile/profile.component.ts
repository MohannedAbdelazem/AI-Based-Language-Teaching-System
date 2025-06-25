import { Component } from '@angular/core';
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


}
