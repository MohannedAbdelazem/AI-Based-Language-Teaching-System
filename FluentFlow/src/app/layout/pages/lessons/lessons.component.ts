import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router, RouterLink } from '@angular/router';
import { TopicsService } from '../../../services/Topics/topics.service';

@Component({
  selector: 'app-lessons',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './lessons.component.html',
  styleUrl: './lessons.component.scss'
})
export class LessonsComponent implements OnInit
{
  title: string = ' ';
  lesson:string = '';
  topicNum: number = 0;
  lessonNum: number = 0;

  constructor(private route:ActivatedRoute, private _TopicsService:TopicsService, private _Router:Router){}
  ngOnInit(): void 
  {
    this.route.params.subscribe(params => {
        this.topicNum = parseInt(params['topicNum'] || '0', 10);
        this.lessonNum = parseInt(params['lessonNum'] || '0', 10);

        if (this._TopicsService.TopicsData.getValue() !== null) {
            const data = this._TopicsService.TopicsData.getValue().topics;
            console.log(data);
            this.title = data[Number(this.topicNum)].topic;
            this.lesson = data[Number(this.topicNum)].lessons[Number(this.lessonNum)];
        }
    });
  }

  getNextLesson(): void
  {
    const data = this._TopicsService.TopicsData.getValue().topics;
    if (this.lessonNum < data[this.topicNum].lessons.length - 1)
    {
      
      this._Router.navigate([`/lessons/${this.topicNum}/${this.lessonNum + 1}`]);
    }
    else if (this.topicNum < data.length - 1)
    {
      this._Router.navigate([`/lessons/${this.topicNum + 1}/0`]);
    }
    else
    {
      this._Router.navigate(['/profile']);
    }
  }

}
