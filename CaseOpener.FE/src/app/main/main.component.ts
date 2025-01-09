import { Component } from '@angular/core';
import { LoaderComponent } from '../shared/loader/loader.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [LoaderComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {

}
