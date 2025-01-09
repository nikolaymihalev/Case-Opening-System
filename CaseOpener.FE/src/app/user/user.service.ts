import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { User } from '../types/user';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  private isAdminSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  
  public isAdmin$: Observable<boolean> = this.isAdminSubject.asObservable();
  public user$: Observable<User | null> = this.userSubject.asObservable();

  constructor(private http: HttpClient) { }

  private setUser(): void {
    const token = localStorage.getItem('authToken');

    if (token) {
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      this.http.get<User>('/api/user/get-user', { headers }).pipe(
        tap((user)=>{
          this.userSubject.next(user);
        })
      );
    }
  }

  private setIsAdmin(): void {
    let userId: string | undefined = '';

    this.userSubject.subscribe((user)=>{
      userId = user?.id;
    })

    this.http.get<boolean>('/api/user/is-admin', { params: { userId } }).pipe(
      tap((message)=>{
        this.isAdminSubject.next(message);
      })
    );
  }

  private clearSubscriptions(): void {
    localStorage.removeItem('authToken');
    this.userSubject.next(null);
    this.isAdminSubject.next(false);
  }

  isAdmin(): boolean {
    return this.isAdminSubject.value;
  }

  isLoggedIn(): boolean {
    return !!this.userSubject.value;
  }

  getCurrentUser(): User | null {
    return this.userSubject.value;
  }

  login(email: string, password: string){
    return this.http
      .post<{token:string}>('/api/user/login', { email, password }, { withCredentials: true })
      .pipe(
        tap((responce)=>{
          if(responce.token){
            localStorage.setItem('authToken', responce.token);

            this.setUser();
            this.setIsAdmin();
          }
        })
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
  
  logout(){
    this.clearSubscriptions();
  }
}
