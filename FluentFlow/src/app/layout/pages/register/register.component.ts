import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { first } from 'rxjs';
import { AuthService } from '../../../services/Auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  constructor(private _AuthService:AuthService, private _Router:Router) 
  {

  }

  registerForm:FormGroup = new FormGroup({
    firstName: new FormControl(),
    lastName: new FormControl(),
    email: new FormControl(),
    password: new FormControl()
  });

  sendData()
  {
    this._AuthService.sendRegister(this.registerForm.value).subscribe({
      next:(res)=>{
        console.log(res);
        this._Router.navigate(['/login']);
      }
      , error:(err)=>{
        console.log(err);
      }
    })
    
  }


}
