import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EmailDirective } from '../../directives/emai.directive';
import { UserValidationConstants } from '../constants/user.validation.constants';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  passwordMinLength = UserValidationConstants.PASSWORD_MIN_LENGTH;
  passwordMaxLength = UserValidationConstants.PASSWORD_MAX_LENGTH;

  emailMinLength = UserValidationConstants.EMAIL_MIN_LENGTH;
  emailMaxLength = UserValidationConstants.EMAIL_MAX_LENGTH;
}
