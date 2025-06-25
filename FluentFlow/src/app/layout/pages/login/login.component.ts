import { Component } from '@angular/core';
import { AuthService } from '../../../services/Auth/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent 
{
  constructor(private _AuthService: AuthService, private _Router: Router) { }


  loginForm:FormGroup = new FormGroup({
      email: new FormControl(),
      password: new FormControl(),
      rememberMe: new FormControl(false)
    });
  
    sendData()
    {
      this._AuthService.sendLogin(this.loginForm.value).subscribe({
        next:(res)=>{
          console.log(res);
          this._Router.navigate(['/flags']);
        }
        , error:(err)=>{
          console.log(err);
        }
      })
      
    }

}
