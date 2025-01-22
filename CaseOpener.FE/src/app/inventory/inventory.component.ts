import { Component, OnInit } from '@angular/core';
import { Item } from '../types/item';
import { User } from '../types/user';
import { UserService } from '../user/user.service';
import { ApiService } from '../shared/api.service';
import { LoaderComponent } from '../shared/loader/loader.component';
import { UserValidationConstants } from '../user/constants/user.validation.constants';
import { NotificationService } from '../shared/notification/notification.service';
import { NotificationComponent } from '../shared/notification/notification.component';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [LoaderComponent, NotificationComponent],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.css'
})
export class InventoryComponent implements OnInit{
  items: Item[] = [];
  currentItems: Item[] = [];
  user: User | undefined;
  currentItem: Item | undefined;

  isLoading: boolean = true;
  isCurrentItemMode: boolean = false;

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  itemsPerPageCount: number = 16;
  pages: number = 0;
  currentPage: number = 1;
  itemsStartIndex: number = 0;
  itemsEndIndex: number = 16;

  constructor(
    private userService: UserService, 
    private apiService: ApiService,
    private notificationService: NotificationService){}

  ngOnInit(): void {
    this.setUser();
    this.getItems();
    this.subscribeToNotification();
  }

  setCurrentItem(item: Item){
    this.currentItem = item;  
    this.setCurrentMode(true);  
  }

  setCurrentMode(operation: boolean){
    this.isCurrentItemMode = operation;

    if(operation === false)
      this.currentItem = undefined;
  }

  sellItem(){
    if(this.currentItem){
      this.userService.updateBalance(this.user?.id!,UserValidationConstants.BALANCE_INCREASE, this.currentItem.amount).subscribe();
      this.apiService.removeItemFromInventory(this.currentItem.id, this.user?.id!).subscribe({
        next: ()=>{
          this.notificationService.showNotification(`Successfully sold the item for ${this.currentItem?.amount}$!`);  
          this.hasNotification = true;
          setTimeout(() => {
            this.setCurrentMode(false);
            this.getItems();
          }, 2000);
        },
        error: ()=>{
          this.notificationService.showNotification('Operation failed! Try again later.', 'error');
          this.hasNotification = true;
        }
      })
    }
  }

  changePage(operation: number){     
    if(operation === 1)  {
        this.itemsEndIndex += this.itemsPerPageCount;
        this.itemsStartIndex += this.itemsPerPageCount;
    }else if(operation === 0){
      this.itemsEndIndex -= this.itemsPerPageCount;
      this.itemsStartIndex -= this.itemsPerPageCount;
    }

    this.getCurrentItems(this.items);

    if(this.currentItems.length === 0){
      this.itemsEndIndex = this.itemsPerPageCount;
      this.itemsStartIndex = 0;
      this.getCurrentItems(this.items);
    }

    this.scrollToTop();
  }

  private scrollToTop(): void {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  private getCurrentItems(items: Item[]){
    this.currentItems = [];
    for (let index = this.itemsStartIndex; index < this.itemsEndIndex; index++) {
      if(items[index])
        this.currentItems.push(items[index]);      
    }    
  }

  private setUser(){
    this.isLoading = true;
    this.userService.user$.subscribe((user)=>{
      this.user = user!;
      this.isLoading = false;
    });
  }  

  private getItems(){
    this.isLoading = true;
    this.apiService.getUserInventoryItems(this.user?.id!).subscribe((items)=>{
      this.items = items;

      this.isLoading = false;
      this.pages = items.length / this.itemsPerPageCount;

      this.getCurrentItems(items);
    })
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
