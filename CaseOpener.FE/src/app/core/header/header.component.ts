import { Component, HostListener, ElementRef } from '@angular/core';
import { Router, RouterLink} from '@angular/router';
import { UserService } from '../../user/user.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent { 
  menuOpen: boolean = false;

  get isLoggedIn(): boolean {
    return this.userService.isLoggedIn();
  }

  get balance(): number{
    let balanceM = 0;
    this.userService.user$.subscribe((user)=>balanceM = user?.balance? user?.balance : 0);
    return balanceM;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.toggleMenu(false);
    }
  }

  constructor(private userService: UserService, private router: Router, private elementRef: ElementRef){}

  logout(): void{
    this.userService.logout();
    this.toggleMenu(false);
    this.router.navigate(['/login']);
  }

  toggleMenu(operation: boolean) {
    this.menuOpen = operation;
  }  
}
