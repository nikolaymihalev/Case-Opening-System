import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { User } from '../../types/user';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { FormsModule, NgForm } from '@angular/forms';
import { UserValidationConstants } from '../constants/user.validation.constants';
import { NotificationService } from '../../shared/notification/notification.service';
import { NotificationComponent } from '../../shared/notification/notification.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [LoaderComponent, FormsModule, NotificationComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  usernameMinLength = UserValidationConstants.USERNAME_MIN_LENGTH;
  usernameMaxLength = UserValidationConstants.USERNAME_MAX_LENGTH;

  user: User | undefined;
  isLoading: boolean = true;
  isEditingMode: boolean = false;

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  constructor(private userService: UserService,private notificationService: NotificationService){}

  ngOnInit(): void {
    this.setUser();
    this.subscribeToNotification();
  }

  get joinedDate():string{
    const dateStr = this.user?.dateJoined!;
    const date = new Date(dateStr);

    const day = date.getDate().toString().padStart(2, "0");
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
  }

  toggleEditMode(){
    this.isEditingMode = !this.isEditingMode;
  }

  saveEdit(form: NgForm){
    if(form.invalid)
      return;

    const{username} = form.value;

    const userId = this.user?.id!;
    
    this.userService.updateInfo(userId, username).subscribe({
      next: ()=>{
        this.isLoading = true;
        this.notificationService.showNotification('You have successfully updated your profile information!');  
        this.hasNotification = true;
        setTimeout(() => {
          this.toggleEditMode();
          this.isLoading = false;
        }, 3000);
      },
      error: ()=>{
        this.notificationService.showNotification('Edit settings operation was not successful. Please check the information you entered and try again!', 'error');
        this.hasNotification = true;
      }
    })
  }

  private setUser(){
    this.isLoading = true;
    this.userService.user$.subscribe((user)=>{
      this.user = user!
      this.isLoading = false;
    });
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
