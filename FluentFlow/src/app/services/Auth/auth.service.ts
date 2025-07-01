import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _HttpClient:HttpClient) { }

  sendRegister(userData:any):Observable<any> 
  {
    return this._HttpClient.post("https://localhost:7051/api/User/register", userData);
  }
  sendLogin(userData:any):Observable<any> 
  {
    return this._HttpClient.post("https://localhost:7051/api/User/login", userData);
  }
}
