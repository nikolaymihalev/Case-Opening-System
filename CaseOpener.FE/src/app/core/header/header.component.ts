import { Component } from '@angular/core';
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
    this.userService.user$.subscribe((user)=>balanceM = user?.balance?user?.balance:0)
    return balanceM;
  }

  constructor(private userService: UserService, private router: Router){}

  logout(): void{
    this.userService.logout();
    this.toggleMenu();
    this.router.navigate(['/login']);
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }  
}
