import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EmailDirective } from '../../directives/emai.directive';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { UserService } from '../user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  passwordMinLength = UserValidationConstants.PASSWORD_MIN_LENGTH;
  passwordMaxLength = UserValidationConstants.PASSWORD_MAX_LENGTH;

  emailMinLength = UserValidationConstants.EMAIL_MIN_LENGTH;
  emailMaxLength = UserValidationConstants.EMAIL_MAX_LENGTH;

  constructor(private userService: UserService, private router: Router){}

  ngOnInit(): void {
    this.checkLoggedIn();
  }

  login(form: NgForm){
    if (form.invalid) {
      return;
    }

    const { email, password } = form.value;

    this.userService.login(email, password).subscribe({
      next:()=>{
        setTimeout(()=>{
          this.router.navigate(['/register']); // CHANGE
        }, 2000);
      },
      error: ()=>{

      }
    });
  }

  private checkLoggedIn(){
    if(this.userService.isLoggedIn())
      this.router.navigate(['/register']); // CHANGE
  }
}
