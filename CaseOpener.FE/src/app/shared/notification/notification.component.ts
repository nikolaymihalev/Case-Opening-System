import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit{
  isVisible: boolean = false;  
  
  @Input() message: string = '';
  @Input() type: string = 'success';

  ngOnInit(): void {
    this.showNotification();
  }

  showNotification(){
    this.isVisible = true;
    setTimeout(()=>{
      this.isVisible = false;
    }, 3000);
  }
}
