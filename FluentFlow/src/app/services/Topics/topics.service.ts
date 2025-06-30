import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TopicsService {

  TopicsData:BehaviorSubject<any> = new BehaviorSubject<any>(null);

  private jsonUrl = '../../../assets/topics.json';
  constructor(private _HttpClient: HttpClient) { }

  getTopics() :Observable<any> 
  {
    const data = this._HttpClient.get(this.jsonUrl);
    data.subscribe((resolvedData) => {
      this.TopicsData.next(resolvedData);
    });
    return data;
  }


}
