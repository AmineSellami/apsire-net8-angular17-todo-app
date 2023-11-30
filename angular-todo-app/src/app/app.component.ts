import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, MatCardModule, MatInputModule,
  MatButtonModule, MatIconModule, MatDividerModule, MatCheckboxModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.sass'
})
export class AppComponent {
  //todos : {} = [] ;
  todos = [{ id: 1, title: 'First Todo', isDone: false, createdAt: new Date() },
  { id: 2, title: 'Second Todo', isDone: true, createdAt: new Date() },
  { id: 3, title: 'Third Todo', isDone: false, createdAt: new Date() }];
}
