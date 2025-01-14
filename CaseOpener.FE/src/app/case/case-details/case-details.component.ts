import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../user/user.service';
import { ApiService } from '../../api.service';
import { CaseDetails } from '../../types/case';
import { LoaderComponent } from '../../shared/loader/loader.component';

@Component({
  selector: 'app-case-details',
  standalone: true,
  imports: [LoaderComponent],
  templateUrl: './case-details.component.html',
  styleUrl: './case-details.component.css'
})
export class CaseDetailsComponent implements OnInit {
  caseDetails: CaseDetails | undefined;
  isLoading: boolean = true;

  constructor(
    private route: ActivatedRoute, 
    private userService: UserService,
    private apiService: ApiService){}

  ngOnInit(): void {
    const id = this.route.snapshot.params['caseId'];

    this.getCase(id);
  }


  private getCase(id: number){
    this.isLoading = true;
    this.apiService.getCase(id).subscribe((data)=>{   
      this.caseDetails = data;
      this.isLoading = false;
    });
  }
}
