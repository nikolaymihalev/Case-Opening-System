import { Component, OnInit } from '@angular/core';
import { CaseUser } from '../../types/case';
import { User } from '../../types/user';
import { UserService } from '../../user/user.service';
import { ApiService } from '../../shared/api.service';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-bought-cases',
  standalone: true,
  imports: [LoaderComponent, RouterLink],
  templateUrl: './bought-cases.component.html',
  styleUrl: './bought-cases.component.css'
})
export class BoughtCasesComponent implements OnInit{
  isLoading: boolean = true;

  cases: CaseUser[] = [];
  user: User | undefined;  
  
  constructor(
    private userService: UserService,
    private apiService: ApiService){}

  ngOnInit(): void {
    this.setUser();
    this.getBoughtCases();
  }

  goToCasePage(caseId: number){

  }

  private setUser(){
    this.isLoading = true;
    this.userService.user$.subscribe((user)=>{
      this.user = user!;
      this.isLoading = false;
    });
  }

  private getBoughtCases(){
    this.apiService.getUserBoughtCases(this.user?.id!).subscribe((cases)=>{
      this.cases = cases;
    })
  }
}
