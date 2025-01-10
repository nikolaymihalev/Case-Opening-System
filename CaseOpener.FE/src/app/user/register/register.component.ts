import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { EmailDirective } from '../../directives/emai.directive';
import { MatchPasswordDirective } from '../../directives/match.passwords.directive';
import { UserService } from '../user.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective, MatchPasswordDirective],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  passwordMinLength = UserValidationConstants.PASSWORD_MIN_LENGTH;
  passwordMaxLength = UserValidationConstants.PASSWORD_MAX_LENGTH;

  emailMinLength = UserValidationConstants.EMAIL_MIN_LENGTH;
  emailMaxLength = UserValidationConstants.EMAIL_MAX_LENGTH;

  usernameMinLength = UserValidationConstants.USERNAME_MIN_LENGTH;
  usernameMaxLength = UserValidationConstants.USERNAME_MAX_LENGTH;

  constructor(private userService: UserService, private router: Router){}

  ngOnInit(): void {
    this.checkLoggedIn();
  }

  register(form: NgForm){
    if(form.invalid)
      return;

    const{username, email, password, confirmPassword} = form.value;

    this.userService.register(username,email,password,confirmPassword).subscribe({
      next: ()=>{
        this.router.navigate(['/login']);
      }
    })
  }

  private checkLoggedIn(){
    if(this.userService.isLoggedIn())
      this.router.navigate(['/home']); // CHANGE
  }
}
