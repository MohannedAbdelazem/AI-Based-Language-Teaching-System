import { GeneralQuestion } from './../../interfaces/generalQuestion/general-question';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  readingQuestions:BehaviorSubject<any> = new BehaviorSubject (null);
  listeningQuestions:BehaviorSubject<any> = new BehaviorSubject (null);
  grammarQuestions:BehaviorSubject<any> = new BehaviorSubject (null);
  questions:BehaviorSubject<any> = new BehaviorSubject(null);
  requieredQuestions: BehaviorSubject<any> = new BehaviorSubject(0);

  constructor(private HttpClient:HttpClient) { }

  getReadingQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/reading.json');
    data.subscribe((resolvedData) => {
      this.readingQuestions.next(resolvedData);
      this.questions.next(resolvedData);
    });
    return data;
  }

  getListeningQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/listening.json');
    data.subscribe((resolvedData) => {
      this.listeningQuestions.next(resolvedData);
      this.questions.next(resolvedData);
    });
    return data;
  }

  getGrammarQuestions():Observable <any> 
  {
    const data = this.HttpClient.get('../../../assets/grammar.json');
    data.subscribe((resolvedData) => {
      this.grammarQuestions.next(resolvedData);
      this.questions.next(resolvedData);
    });
    return data;
  }
}
