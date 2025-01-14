import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { User } from '../types/user';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);  

  public user$: Observable<User | null> = this.userSubject.asObservable();

  constructor(private http: HttpClient) { 
    this.setUser();
  }

  private setUser(): void {    
    const token = localStorage.getItem('authToken');

    if (token) {
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      this.http.get<User>('/api/user/get-user', { headers }).pipe(
        tap((user)=>{
          this.userSubject.next(user);  
        })
      ).subscribe();
    }
  }

  private clearSubscriptions(): void {
    localStorage.removeItem('authToken');
    this.userSubject.next(null);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('authToken');
    return !!this.user$ && !!token;
  }

  login(email: string, password: string){
    return this.http
      .post<{token:string}>('/api/user/login', { email, password }, { withCredentials: true })
      .pipe(
        tap((responce)=>{
          if(responce.token){
            localStorage.setItem('authToken', responce.token);

            this.setUser();   
          }
        },
        catchError((err: HttpErrorResponse)=>{          
          return throwError(() => new Error(err.error));
        }))
      )
  }
  
  register(username: string, email: string, password: string, confirmPassword: string){
    return this.http
      .post('/api/user/register', {username, email, password, confirmPassword}, { withCredentials: true })
      .pipe(
        catchError((err: HttpErrorResponse)=>{          
          return throwError(() => new Error(err.error));
        })
      );
  }

  updateInfo(userId: string, username: string){
    return this.http
      .put('/api/user/update-info', null, {params:{userId, username}, withCredentials: true })
      .pipe( catchError((err: HttpErrorResponse)=>{          
        return throwError(() => new Error(err.error));
      }));
  }
  
  logout(){
    this.clearSubscriptions();
  }
}
