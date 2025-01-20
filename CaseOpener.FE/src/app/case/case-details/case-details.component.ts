import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../user/user.service';
import { CaseDetails, CaseItem } from '../../types/case';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { User } from '../../types/user';
import { NotificationService } from '../../shared/notification/notification.service';
import { UserValidationConstants } from '../../user/constants/user.validation.constants';
import { NotificationComponent } from '../../shared/notification/notification.component';
import { ApiService } from '../../shared/api.service';
import { Item } from '../../types/item';

@Component({
  selector: 'app-case-details',
  standalone: true,
  imports: [LoaderComponent, NotificationComponent],
  templateUrl: './case-details.component.html',
  styleUrl: './case-details.component.css'
})
export class CaseDetailsComponent implements OnInit {
  caseDetails: CaseDetails | undefined;
  user: User | undefined;
  quantityArray: number[] = [1, 2, 3, 4, 5];
  
  isLoading: boolean = true;
  isOpening: boolean = false;
  isCaseOpened: boolean = false;
  doesUserHaveCaseCount: number = 0;

  caseQuantity: number = 1;
  casePrice: number = 0;

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  displayedItems: Item[] = [];
  items: number[] = Array.from({ length: 15 }, (_, i) => i);
  stopAt: number = 12; 
  positions: number[] = this.items.map(() => 100); 
  openedItem: Item | undefined;

  get getCasePrice(): number{
    return this.casePrice*this.caseQuantity;
  }

  get caseId(): number{
    return this.route.snapshot.params['caseId'];
  }

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private apiService: ApiService, 
    private notificationService: NotificationService){}

  ngOnInit(): void {
    this.setUser();
    this.getCase(this.caseId);
    this.checkDoesUserHaveCase(this.caseId);
    this.subscribeToNotification();    
    
  }

  buy(){    
    if(this.canUserBuyCase() === true){
      this.apiService.buyCase(this.caseDetails?.case.id!, this.user?.id!, this.caseQuantity).subscribe({
        next:()=>{
          this.notificationService.showNotification('Successfully bought case!');
          this.hasNotification = true;
  
          this.checkDoesUserHaveCase(this.caseDetails?.case.id!);
          this.updateUserBalance();
        },
        error: ()=>{
          this.notificationService.showNotification('Operation failed! Try again later', 'error');
          this.hasNotification = true;
        }
      })
    } else {   
      let notMessage = this.user !== undefined && this.user !== null ? 'Not enough money in your balance!' : 'Please log in to your account to purchase a case.';            
      let notKind = this.user !== undefined && this.user !== null ? 'error' : 'warning';
      this.notificationService.showNotification(notMessage, notKind);
      this.hasNotification = true;
    }
  }
  
  changeCaseQuantity(quantity: number){    
    this.caseQuantity = quantity;    
  }

  canUserBuyCase():boolean{    
    return this.user?.balance! >= this.casePrice;
  }

  startOpening() {
    this.isOpening = true;
    this.apiService.getCaseProbabilities(this.caseDetails?.case.id!).subscribe((probabilities)=>{      
      this.generateDisplayedItems(probabilities);
    })

    this.apiService.openCase(this.caseDetails?.case.id!, this.user?.id!).subscribe((item: Item) => {
      this.displayedItems[this.stopAt] = item;
      this.checkDoesUserHaveCase(this.caseId);       
    })

    this.animateItems();
  }

  stopOpenning(){
    this.notificationService.showNotification('Added item to inventory!');
    this.hasNotification = true;
    this.isLoading = true;
    setTimeout(()=>{
      this.isCaseOpened = false; 
      this.isOpening = false;
      this.isLoading = false;
    },1500);
  }

  getAnimationStyle(index: number): { transform: string, backgroundColor: string,  borderColor: string} {
    return {      
      transform: `translateX(${this.positions[index]}%)`,
      backgroundColor: this.displayedItems[index].rarity === 'MilSpec' ? '#000024':
        this.displayedItems[index].rarity === 'Restricted' ? '#13002b':
        this.displayedItems[index].rarity === 'Classified' ? '#2b0017' :
        this.displayedItems[index].rarity === 'Covert'? '#2b0000': '#2b2000',
      borderColor: this.displayedItems[index].rarity === 'MilSpec' ? 'darkblue':
        this.displayedItems[index].rarity === 'Restricted' ? 'purple':
        this.displayedItems[index].rarity === 'Classified' ? 'plum' :
        this.displayedItems[index].rarity === 'Covert'? 'red': 'gold'
    };
  }

  private generateDisplayedItems(probabilities: CaseItem[]) {
    this.displayedItems = [];

    for (let i = 0; i < this.items.length; i++) {
      const randomItem = this.getWeightedRandomItem(probabilities);      
      const itemDetails = this.caseDetails?.items.find(item => item.id === randomItem.itemId); 
      if (itemDetails) {
        this.displayedItems.push(itemDetails);
      }
    }       
  }

  private animateItems() {
    this.displayedItems.forEach((_, index) => {
        this.positions[index] -= 15;
    });
    
    if (this.positions[this.stopAt] <= -1000) {
      setTimeout(()=>{
        this.isCaseOpened = true; 
        this.openedItem = this.displayedItems[this.stopAt];
      }, 2000);
    } else {
      setTimeout(() => this.animateItems(), 50);
    }
  }

  private getWeightedRandomItem(probabilities: CaseItem[]): CaseItem {
    const totalProbability = probabilities.reduce((sum, item) => sum + item.probability, 0);
    
    const randomValue = Math.random() * totalProbability;
  
    let cumulativeProbability = 0;
    for (const item of probabilities) {
      cumulativeProbability += item.probability;
      if (randomValue <= cumulativeProbability) {
        return item;
      }
    }
    return probabilities[probabilities.length - 1];
  }
  
  private updateUserBalance(){
    this.userService.updateBalance(this.user?.id!, UserValidationConstants.BALANCE_DECREASE, this.getCasePrice).subscribe();
  }

  private getCase(id: number){
    this.isLoading = true;
    this.apiService.getCase(id).subscribe((data)=>{   
      this.caseDetails = data;
      this.casePrice = data.case.price;
      this.isLoading = false;
    });
  }

  private setUser(){
    this.isLoading = true;
    this.userService.user$.subscribe((user)=>{
      this.user = user!;
      this.isLoading = false;
    });
  }

  private checkDoesUserHaveCase(caseId: number){
    this.apiService.doesUserHaveCase(caseId, this.user?.id!).subscribe((responce)=>{
      this.doesUserHaveCaseCount = responce;      
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
