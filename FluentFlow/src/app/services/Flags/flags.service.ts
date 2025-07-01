import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FlagsService {

  constructor(private _HttpClient: HttpClient) { }

  // Example flag
  getFlags(): Observable<any>
  {

    return this._HttpClient.get("https://restcountries.com/v3.1/all?fields=flags");
  }
}
