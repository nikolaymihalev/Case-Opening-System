import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { Item } from '../types/item';
import { User } from '../types/user';
import { UserService } from '../user/user.service';
import { ApiService } from '../shared/api.service';
import { LoaderComponent } from '../shared/loader/loader.component';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [LoaderComponent],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.css'
})
export class InventoryComponent implements OnInit{
  items: Item[] = [];
  user: User | undefined;
  currentItem: Item | undefined;

  isLoading: boolean = true;
  isCurrentItemMode: boolean = false;

  constructor(
    private userService: UserService, 
    private apiService: ApiService,
  private elementRef: ElementRef){}

  ngOnInit(): void {
    this.setUser();
    this.getItems();
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
    })
  }
}
