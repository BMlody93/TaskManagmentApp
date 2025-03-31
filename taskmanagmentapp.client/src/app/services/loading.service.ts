import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private ongoingRequests = 0;
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();

  show() {
    this.ongoingRequests++;
    this.updateLoadingState();
  }

  hide() {
    this.ongoingRequests--;
    this.updateLoadingState();
  }

  private updateLoadingState() {
    // Show the spinner if there are active requests, hide if there are none
    this.loadingSubject.next(this.ongoingRequests > 0);
  }
}
