import { Component } from '@angular/core';
import { CaseListComponent } from '../case/case-list/case-list.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CaseListComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
}
