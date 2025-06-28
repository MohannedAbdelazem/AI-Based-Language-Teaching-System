import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  private questions:BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private HttpClient:HttpClient) { }

  getReadingQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/reading.json');
    this.questions.next(data);
    return data;
  }

  getListeningQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/listening.json');
    this.questions.next(data);
    return data;
  }

  getGrammarQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/grammar.json');
    this.questions.next(data);
    return data;
  }
}
