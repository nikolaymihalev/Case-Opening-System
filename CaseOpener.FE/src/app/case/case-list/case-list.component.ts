import { Component, OnInit } from '@angular/core';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { Case } from '../../types/case';
import { RouterLink } from '@angular/router';
import { Category } from '../../types/category';
import { SlicePipe } from '../../shared/pipes/slice.pipe';
import { FormsModule, NgForm } from '@angular/forms';
import { ApiService } from '../../api.service';

@Component({
  selector: 'app-case-list',
  standalone: true,
  imports: [LoaderComponent, RouterLink, SlicePipe, FormsModule],
  templateUrl: './case-list.component.html',
  styleUrl: './case-list.component.css'
})
export class CaseListComponent implements OnInit{  
  isLoading: boolean = true;
  isSearching: boolean = false;
  noResults: boolean = false;

  categories: Category[] = [];

  weaponCases: Case[] = [];
  operationCases: Case[] = [];
  rareCases: Case[] = [];
  graffitiCases: Case[] = [];
  stickerCases: Case[] = [];

  constructor(private apiService: ApiService){}

  ngOnInit(): void {
    this.getCategories();
    this.getAllCases();
  }

  scrollToCategory(categoryName: string) {
    const targetElement = document.getElementById(`${categoryName}-cases`);
    if (targetElement) {
      targetElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  toggleSearchMode(){
    this.isSearching = !this.isSearching;
  }

  searchCases(form: NgForm){
    if(form.invalid)
      return;

    const {search} = form.value;
    
    this.getAllCases(search);
    this.toggleSearchMode();
  }

  backToHome(){
    this.getAllCases();
  }

  private getAllCases(name?:string){
    this.isLoading = true;

    if(name){
      this.apiService.searchCases(name).subscribe((cases)=>{
        this.weaponCases = cases.filter(x=>x.categoryName=="Weapon");
        this.operationCases = cases.filter(x=>x.categoryName=="Operation");
        this.rareCases = cases.filter(x=>x.categoryName=="Rare");
        this.graffitiCases = cases.filter(x=>x.categoryName=="Graffiti");
        this.stickerCases = cases.filter(x=>x.categoryName=="Sticker");

        this.noResults = cases.length === 0;
      });
    }else{
      this.apiService.getAllCases().subscribe((cases)=>{
        this.weaponCases = cases.filter(x=>x.categoryName=="Weapon");
        this.operationCases = cases.filter(x=>x.categoryName=="Operation");
        this.rareCases = cases.filter(x=>x.categoryName=="Rare");
        this.graffitiCases = cases.filter(x=>x.categoryName=="Graffiti");
        this.stickerCases = cases.filter(x=>x.categoryName=="Sticker");

        this.noResults = cases.length === 0;
      });
    }
    this.isLoading = false;
  }

  private getCategories(){
    this.isLoading = true;

    this.apiService.getAllCategories().subscribe((categories)=>this.categories = categories.filter(x=>x.name==='Weapon'||x.name==='Operation'||x.name=="Rare"||x.name=="Graffiti"||x.name=="Sticker"));

    this.isLoading = false;
  }
}
