import { Component, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TaskItemComponent } from '../task-item/task-item.component'

import { Task, PagedTaskData } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TaskItemComponent],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  @Input() tasks: PagedTaskData | null = null;
  @Input() isModifiable: boolean = true;

  @Output() pageChanged = new EventEmitter<number>();

  pageNumber: number = 1; // Default to 1

  //Updates pageNumber when tasks have changed
  ngOnChanges(changes: SimpleChanges) {
    if (changes['tasks'] && this.tasks) {
      this.pageNumber = this.tasks.pageIndex + 1; 
    }
  }

  get totalPages(): number {
    return this.tasks ? Math.ceil(this.tasks.totalItems / this.tasks.pageSize) : 1;
  }

  //Emits event when page is changed
  onPageChange(pageNumber: number) {
    if (pageNumber >= 1 && pageNumber <= this.totalPages) {
      this.pageChanged.emit(pageNumber - 1); // Emit zero-based index
    }
  }
}
