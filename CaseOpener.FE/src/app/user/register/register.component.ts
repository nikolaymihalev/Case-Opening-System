import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { EmailDirective } from '../../directives/emai.directive';
import { MatchPasswordDirective } from '../../directives/match.passwords.directive';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective, MatchPasswordDirective],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  passwordMinLength = UserValidationConstants.PASSWORD_MIN_LENGTH;
  passwordMaxLength = UserValidationConstants.PASSWORD_MAX_LENGTH;

  emailMinLength = UserValidationConstants.EMAIL_MIN_LENGTH;
  emailMaxLength = UserValidationConstants.EMAIL_MAX_LENGTH;

  usernameMinLength = UserValidationConstants.USERNAME_MIN_LENGTH;
  usernameMaxLength = UserValidationConstants.USERNAME_MAX_LENGTH;
}
