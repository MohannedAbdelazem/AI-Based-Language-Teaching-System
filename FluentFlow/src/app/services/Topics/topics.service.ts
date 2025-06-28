import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TopicsService {

  private jsonUrl = '../../../assets/topics.json';
  constructor(private _HttpClient: HttpClient) { }

  getTopics() :Observable<any> 
  {
    return this._HttpClient.get(this.jsonUrl);
  }


}
