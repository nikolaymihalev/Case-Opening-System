import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationSource = new BehaviorSubject<{ message: string, type: string }>({ message: '', type: 'success' });
  notification$ = this.notificationSource.asObservable();

  showNotification(message: string, type: string = 'success') {
    this.notificationSource.next({ message, type });
  }
}
