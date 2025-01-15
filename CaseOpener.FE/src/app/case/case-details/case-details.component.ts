import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../user/user.service';
import { CaseDetails } from '../../types/case';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { User } from '../../types/user';
import { NotificationService } from '../../shared/notification/notification.service';
import { UserValidationConstants } from '../../user/constants/user.validation.constants';
import { NotificationComponent } from '../../shared/notification/notification.component';
import { ApiService } from '../../shared/api.service';

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
  doesUserHaveCaseCount: number = 0;

  caseQuantity: number = 1;
  casePrice: number = 0;

  notificationMessage: string = '';
  notificationType: string = '';
  hasNotification: boolean = false;

  get getCasePrice(): number{
    return this.casePrice*this.caseQuantity;
  }

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private apiService: ApiService, 
    private notificationService: NotificationService){}

  ngOnInit(): void {
    const id = this.route.snapshot.params['caseId'];

    this.setUser();
    this.getCase(id);
    this.checkDoesUserHaveCase(id);
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
      this.notificationService.showNotification('Not enough money in your balance!', 'error');
      this.hasNotification = true;
    }
  }
  
  changeCaseQuantity(quantity: number){    
    this.caseQuantity = quantity;    
  }

  canUserBuyCase():boolean{    
    return this.user?.balance! >= this.casePrice;
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
