import { Component, OnInit } from '@angular/core';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { Case } from '../../types/case';
import { ApiService } from '../api.service';
import { RouterLink } from '@angular/router';
import { Category } from '../../types/category';
import { SlicePipe } from '../../shared/pipes/slice.pipe';

@Component({
  selector: 'app-case-list',
  standalone: true,
  imports: [LoaderComponent, RouterLink, SlicePipe],
  templateUrl: './case-list.component.html',
  styleUrl: './case-list.component.css'
})
export class CaseListComponent implements OnInit{  
  isLoading: boolean = true;
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

  private getAllCases(){
    this.isLoading = true;

    this.apiService.getAllCases().subscribe((cases)=>{
      console.log(cases);
      
      this.weaponCases = cases.filter(x=>x.categoryName=="Weapon");
      this.operationCases = cases.filter(x=>x.categoryName=="Operation");
      this.rareCases = cases.filter(x=>x.categoryName=="Rare");
      this.graffitiCases = cases.filter(x=>x.categoryName=="Graffiti");
      this.stickerCases = cases.filter(x=>x.categoryName=="Sticker");
    });
    this.isLoading = false;
  }

  private getCategories(){
    this.isLoading = true;

    this.apiService.getAllCategories().subscribe((categories)=>this.categories = categories.filter(x=>x.name==='Weapon'||x.name==='Operation'||x.name=="Rare"||x.name=="Graffiti"||x.name=="Sticker"));

    this.isLoading = false;
  }
}
