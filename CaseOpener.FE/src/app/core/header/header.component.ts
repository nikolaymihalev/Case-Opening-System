import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../../user/user.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit { 
  isLoggedIn: boolean = false;

  constructor(private userService: UserService){}

  ngOnInit(): void {
    this.isLoggedIn = this.userService.isLoggedIn();    
  }
}
