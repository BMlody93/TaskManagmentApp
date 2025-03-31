import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { UserApiService } from './services/user-api.service';
import { TaskApiService } from './services/task-api.service';
import { CacheService } from './services/cache.service';
import { LoadingService } from './services/loading.service';

import { UserSelectorComponent } from './user-selector/user-selector.component';
import { TaskListComponent } from './task/task-list/task-list.component';

import { User } from './models/user.model';
import { Task, PagedTaskData } from './models/task.model';

import { InfoPopupComponent } from './popups/info-popup/info-popup.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    UserSelectorComponent,
    TaskListComponent,
    CommonModule,
    FormsModule,
    MatDialogModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  users: User[] = [];
  tasksAssigned: PagedTaskData | null = null;
  tasksUnassigned: PagedTaskData | null = null;
  selectedUser: User | null = null;
  assignedPageIndex = 0;
  unassignedPageIndex = 0;
  pageSize = 10;
  isSaveDisabled = true;

  constructor(
    private userApiService: UserApiService,
    private taskApiService: TaskApiService,
    private cacheService: CacheService,
    public loadingService: LoadingService,
    private dialog: MatDialog) { }

  newAssignedTaskIds: number[] = [];

  ngOnInit() {
    // Fetch users when the component initializes
    this.userApiService.getUsers().subscribe(
      (users) => {
        this.users = users;
      }
    );
  }

  // Fetch tasks for the selected user
  onUserSelected(user: User) {
    this.selectedUser = user;

    //sets new current user in cache and clears new assignedTasks
    this.cacheService.set("currentUser", user);
    this.cacheService.remove("newAssignedTasks");

    this.fetchAssignedTasks(user.id, true);
    this.fetchUnassignedTasks();

    this.isSaveDisabled = false;
  }

  // Method to fetch assigned tasks for the selected user
  fetchAssignedTasks(userId: number, withStatistics: boolean = false) {
    this.taskApiService.getAssignedTasks(userId, this.assignedPageIndex, this.pageSize, withStatistics).subscribe((tasks) => {
      this.tasksAssigned = tasks;
      if (withStatistics)
      //if request was asking for statistics the are set to chache to be used when validating assigning task to user
        this.cacheService.set("statistics", tasks.statistics)
    });
  }

  // Method to fetch unassigned tasks
  fetchUnassignedTasks() {
    this.taskApiService.getUnassignedTasks(this.unassignedPageIndex, this.pageSize).subscribe((tasks) => {
      this.tasksUnassigned = tasks;
    });
  }

  // Handle page change for assigned tasks
  onAssignedPageChange(pageIndex: number) {
    if (this.selectedUser) {
      this.assignedPageIndex = pageIndex;
      this.fetchAssignedTasks(this.selectedUser.id); // Re-fetch assigned tasks when page changes
    }
  }

  // Handle page change for unassigned tasks
  onUnassignedPageChange(pageIndex: number) {
    this.unassignedPageIndex = pageIndex;
    this.fetchUnassignedTasks(); // Re-fetch unassigned tasks when page changes
  }

  //Validates and then sends request to assign tasks
  onAssignTasks() {
    var userId = this.selectedUser?.id;
    var newAssignedTasks = this.cacheService.get("newAssignedTasks") as Task[];
    if (!newAssignedTasks) {
      this.dialog.open(InfoPopupComponent, {
        width: '300px',
        data: { message: 'There are no new assigned tasks' }
      })
      return;
    }


    var statistics = this.cacheService.get("statistics") as Record<string, any>
    if (userId && statistics) {

      var newTasksCount = newAssignedTasks.length
      if (newTasksCount > 10) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'There are too many new assigned tasks. If you want to add new, first save current changes' }
        })
        return;
      }

      var newTotal = newAssignedTasks.length + (statistics["TotalTasksCount"] as number)
      if (newTotal > 11) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'Task limit has been reached for this user' }
        })
        return;
      }
      if (newTotal < 5) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'User does not have enough tasks' }
        })
        return;
      }

      var hardTaskCount = (statistics["HardTasksCount"] as number) +
        newAssignedTasks.filter(t => t.difficulty == 4 || t.difficulty == 5).length;
      var hardTaskPercentage = (hardTaskCount / newTotal) * 100;
      if (hardTaskPercentage > 30) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'There are too many hard tasks for this user' }
        })
        return;
      }
      if (hardTaskPercentage < 10) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'There are not enough hard tasks for this user' }
        })
        return;
      }

      var easyTaskCount = (statistics["EasyTasksCount"] as number) +
        newAssignedTasks.filter(t => t.difficulty == 1 || t.difficulty == 2).length;
      var easyTaskPercentage = (easyTaskCount / newTotal) * 100;
      if (easyTaskPercentage > 50) {
        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'There are too many easy tasks for this user' }
        })
        return;
      }


      this.taskApiService.assignTasks(userId, newAssignedTasks.map(task => task.id)).subscribe((result) => {

        this.cacheService.remove("newAssignedTasks");

        this.fetchAssignedTasks(userId!, true);
        this.fetchUnassignedTasks();

        this.dialog.open(InfoPopupComponent, {
          width: '300px',
          data: { message: 'Assigned tasks to user.' }
        })
      });
    }
  }
}
