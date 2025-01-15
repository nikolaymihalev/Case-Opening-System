import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../user/user.service';
import { ApiService } from '../../api.service';
import { CaseDetails } from '../../types/case';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { User } from '../../types/user';

@Component({
  selector: 'app-case-details',
  standalone: true,
  imports: [LoaderComponent],
  templateUrl: './case-details.component.html',
  styleUrl: './case-details.component.css'
})
export class CaseDetailsComponent implements OnInit {
  caseDetails: CaseDetails | undefined;
  user: User | undefined;
  
  isLoading: boolean = true;
  doesUserHaveCase: boolean = false;

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private apiService: ApiService){}

  ngOnInit(): void {
    const id = this.route.snapshot.params['caseId'];

    this.setUser();
    this.getCase(id);
    this.checkDoesUserHaveCase(id)
  }


  private getCase(id: number){
    this.isLoading = true;
    this.apiService.getCase(id).subscribe((data)=>{   
      this.caseDetails = data;
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
    this.apiService.doesUserHaveCase(caseId, this.user?.id!).subscribe((message)=>{
      if(message.message=='False'){
        this.doesUserHaveCase = false;
      }else{
        this.doesUserHaveCase = true;
      }        
    })
  }
}
