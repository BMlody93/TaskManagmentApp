import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  constructor(private http: HttpClient) { }

  //Method to get users
  getUsers(): Observable<User[]> {
    return this.http.get<any[]>(`api/User`);
  }
}
