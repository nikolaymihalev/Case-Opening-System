import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Case, CaseDetails } from './types/case';
import { Category } from './types/category';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getAllCases(){
    return this.http.get<Case[]>('/api/case/all');
  }

  searchCases(name: string){
    return this.http.get<Case[]>('/api/case/search', {params: {name: name}});
  }

  getAllCategories(){
    return this.http.get<Category[]>('/api/category/all');
  }

  getCase(id: number){
    return this.http.get<CaseDetails>('/api/case/get-case', {params: {id: id}});
  }
}
