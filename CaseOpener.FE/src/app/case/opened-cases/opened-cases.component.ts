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
  user: User | undefined;

  isLoading: boolean = true;

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
    })
  }
}
