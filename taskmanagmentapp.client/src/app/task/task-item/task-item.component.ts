import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskTypePipe, StatusPipe } from '../../pipes/task-pipes';

import { CacheService } from '../../services/cache.service';

import { Task, TaskType } from '../../models/task.model';
import { User, UserType } from '../../models/user.model';

@Component({
  selector: 'app-task-item',
  imports: [CommonModule, TaskTypePipe, StatusPipe ],
  templateUrl: './task-item.component.html',
  styleUrl: './task-item.component.css'
})
export class TaskItemComponent {
  @Input() task: Task | null = null;
  @Input() isModifiable: boolean = true;

  showDetails: boolean = false;
  isAssigned: boolean = false;
  taskTypeEnum = TaskType

  constructor(private cacheService: CacheService) { }


  //on initialization checks if task checkbox on task item should be disabled and checked
  ngOnInit() {
    this.isAssigned = this.task?.assignedUserId != null

    var selectedUser = this.cacheService.get("currentUser") as User;
    this.isModifiable = this.isModifiable && !(this.task?.taskType !== TaskType.Implementation && selectedUser?.userType === UserType.Programmer)

    var newAssignedTasks = this.cacheService.get("newAssignedTasks") as Task[];
    if (newAssignedTasks && this.task)
      this.isAssigned = newAssignedTasks.findIndex(t=>t.id === this.task!.id) !== -1;
  }

  //stores task in cache to mark it as assigned to user
  //latter list of new assigned task is used to sent data to web api
  toggleAssigment() {
    this.isAssigned = !this.isAssigned;

    var newAssignedTasks = this.cacheService.get("newAssignedTasks") as Task[];
    var statistics = this.cacheService.get("statistics") as Record<string, any>

    if (!newAssignedTasks)
      newAssignedTasks = [];

    if (this.task) {
      const index = newAssignedTasks.findIndex(t=>t.id === this.task!.id);

      if (index === -1) {
        newAssignedTasks.push(this.task);
      } else {
        newAssignedTasks.splice(index, 1);
      }

      this.cacheService.set("newAssignedTasks", newAssignedTasks);
    }
  }

  //sets if accordeon should be rolled out or in
  toggleDetails() {
    this.showDetails = !this.showDetails;
  }
}
