import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EmailDirective } from '../../directives/emai.directive';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { UserService } from '../user.service';
import { NotificationComponent } from '../../shared/notification/notification.component';
import { NotificationService } from '../../shared/notification/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, EmailDirective, NotificationComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  passwordMinLength = UserValidationConstants.PASSWORD_MIN_LENGTH;
  passwordMaxLength = UserValidationConstants.PASSWORD_MAX_LENGTH;

  emailMinLength = UserValidationConstants.EMAIL_MIN_LENGTH;
  emailMaxLength = UserValidationConstants.EMAIL_MAX_LENGTH;

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  constructor(
    private userService: UserService, 
    private router: Router, 
    private notificationService: NotificationService){}

  ngOnInit(): void {
    this.checkLoggedIn();
    this.subscribeToNotification();
  }

  login(form: NgForm){
    if (form.invalid) {
      return;
    }

    const { email, password } = form.value;

    this.userService.login(email, password).subscribe({
      next:()=>{
        this.notificationService.showNotification('Successfully logged in!');
        this.hasNotification = true;

        setTimeout(()=>{
          this.router.navigate(['/home']);
        }, 3000);

      },
      error: ()=>{
        this.notificationService.showNotification('The email or password is invalid!', 'error');
        this.hasNotification = true;
      }
    });
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
