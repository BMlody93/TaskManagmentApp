import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedTaskData } from '../models/task.model'

@Injectable({
  providedIn: 'root'
})
export class TaskApiService {

  constructor(private http: HttpClient) { }

  // Method to fetch assigned tasks for a specific user
  getAssignedTasks(userId: number, pageIndex: number, pageSize: number, withStatistics: boolean): Observable<PagedTaskData> {
    const url = `api/Task/${userId}?pageIndex=${pageIndex}&pageSize=${pageSize}&withStatistics=${withStatistics}`;
    return this.http.get<PagedTaskData>(url);
  }

  // Method to fetch unassigned tasks
  getUnassignedTasks(pageIndex: number, pageSize: number): Observable<PagedTaskData> {
    const url = `api/Task?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<PagedTaskData>(url);
  }

  // Method to assign tasks to user
  assignTasks(userId: number, taskIds: number[]): Observable<any> {
    return this.http.put(`api/Task/${userId}`, taskIds);
  }
}
