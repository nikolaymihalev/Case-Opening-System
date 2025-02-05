import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { EmailDirective } from '../../directives/emai.directive';
import { MatchPasswordDirective } from '../../directives/match.passwords.directive';
import { UserService } from '../user.service';
import { NotificationService } from '../../shared/notification/notification.service';
import { NotificationComponent } from '../../shared/notification/notification.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective, MatchPasswordDirective, NotificationComponent],
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

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  constructor(
    private userService: UserService, 
    private notificationService: NotificationService,
    private router: Router){}

  ngOnInit(): void {
    this.checkLoggedIn();
    this.subscribeToNotification();
  }

  register(form: NgForm){
    if(form.invalid)
      return;

    const{username, email, password, confirmPassword} = form.value;

    this.userService.register(username,email,password,confirmPassword).subscribe({
      next: ()=>{
        this.notificationService.showNotification('You have successfully registered!');  
        this.hasNotification = true;
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 3000);
      },
      error: ()=>{
        this.notificationService.showNotification('Registration was not successful. Please check the information you entered and try again!', 'error');
        this.hasNotification = true;
      }
    })
  }

  private checkLoggedIn(){
    if(this.userService.isLoggedIn())
      this.router.navigate(['/settings']);
  }

  private subscribeToNotification(): void{
    this.notificationService.notification$.subscribe(notification => {
      this.notificationMessage = notification.message;
      this.notificationType = notification.type;
      setTimeout(() => {
        this.notificationMessage = '';
        this.hasNotification = false;
      }, 5000);
    });
  }
}
