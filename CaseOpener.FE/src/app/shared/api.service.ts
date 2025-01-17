import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Case, CaseDetails, CaseItem, CaseUser, OpenedCase } from '../types/case';
import { Category } from '../types/category';
import { Item } from '../types/item';

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

  doesUserHaveCase(caseId: number, userId: string){
    return this.http.get<number>('/api/case/user-has-case', {params: {caseId, userId}});
  }

  buyCase(caseId: number, userId: string, quantity: number){
    return this.http
      .post('/api/case/buy-case', null, {params:{caseId, userId, quantity}, withCredentials: true })
      .pipe(
        catchError((err: HttpErrorResponse)=>{          
          return throwError(() => new Error(err.error));
        })
      );
  }

  getUserBoughtCases(userId: string){
    return this.http.get<CaseUser[]>('/api/case/bought-cases', {params: {userId}})
    .pipe(
      catchError((err: HttpErrorResponse)=>{          
        return throwError(() => new Error(err.error));
      })
    );
  }

  openCase(caseId: number, userId: string){
    return this.http
    .post<Item>('/api/case/open', null, {params: {caseId, userId}, withCredentials: true})
    .pipe(
      catchError((err: HttpErrorResponse)=>{          
        return throwError(() => new Error(err.error));
      })
    );
  }

  getCaseProbabilities(caseId: number){
    return this.http.get<CaseItem[]>('/api/case/item-probabilities', {params: {caseId}});
  }

  getUserOpenedCases(userId: string){
    return this.http.get<OpenedCase[]>('/api/case/user-opened-cases', {params: {userId}});
  }

  getUserInventoryItems(userId: string){
    return this.http.get<Item[]>('/api/item/user-items', {params: {userId}});
  }
}
