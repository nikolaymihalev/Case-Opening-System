import { Component, OnInit } from '@angular/core';
import { OpenedCase } from '../../types/case';
import { User } from '../../types/user';
import { UserService } from '../../user/user.service';
import { ApiService } from '../../shared/api.service';
import { LoaderComponent } from "../../shared/loader/loader.component";

@Component({
  selector: 'app-opened-cases',
  standalone: true,
  imports: [LoaderComponent],
  templateUrl: './opened-cases.component.html',
  styleUrl: './opened-cases.component.css'
})
export class OpenedCasesComponent implements OnInit{
  openedCases: OpenedCase[] = [];
  currentOpenedCases: OpenedCase[] = [];
  user: User | undefined;

  isLoading: boolean = true;

  openedCasesPerPageCount: number = 5;
  pages: number = 0;
  currentPage: number = 1;
  openedCasesStartIndex: number = 0;
  openedCasesEndIndex: number = 5;

  constructor(
    private userService: UserService,
    private apiService: ApiService){}

  ngOnInit(): void {
    this.setUser();
    this.getCases();
  }

  getOpenedDate(openedDate: string):string{
    const dateStr = openedDate;
    const date = new Date(dateStr);

    const day = date.getDate().toString().padStart(2, "0");
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const year = date.getFullYear();
    const hour = date.getHours().toString().padStart(2, "0");
    const minutes = date.getMinutes().toString().padStart(2, "0");

    return `${day}/${month}/${year} - ${hour}:${minutes}`;
  }

  changePage(operation: number){     
    if(operation === 1)  {
        this.openedCasesEndIndex += this.openedCasesPerPageCount;
        this.openedCasesStartIndex += this.openedCasesPerPageCount;
    }else if(operation === 0){
      this.openedCasesEndIndex -= this.openedCasesPerPageCount;
      this.openedCasesStartIndex -= this.openedCasesPerPageCount;
    }

    this.getCurrentItems(this.openedCases);

    if(this.currentOpenedCases.length === 0){
      this.openedCasesEndIndex = this.openedCasesPerPageCount;
      this.openedCasesStartIndex = 0;
      this.getCurrentItems(this.openedCases);
    }

    this.scrollToTop();
  }

  private scrollToTop(): void {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }
  
  private getCurrentItems(openedCases: OpenedCase[]){
    this.currentOpenedCases = [];
    for (let index = this.openedCasesStartIndex; index < this.openedCasesEndIndex; index++) {
      if(openedCases[index])
        this.currentOpenedCases.push(openedCases[index]);      
    }    
  }

  private setUser(){
    this.isLoading = true;
    this.userService.user$.subscribe((user)=>{
      this.user = user!;
      this.isLoading = false;
    });
  }

  private getCases(){
    this.isLoading = true;
    this.apiService.getUserOpenedCases(this.user?.id!).subscribe((cases)=>{
      this.openedCases = cases;

      this.isLoading = false;

      this.pages = cases.length / this.openedCasesPerPageCount;
      this.getCurrentItems(cases);
    })
  }
}
