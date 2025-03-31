import { Pipe, PipeTransform } from '@angular/core';
import { TaskType, Status } from '../models/task.model';

@Pipe({
  name: 'taskType'
})
export class TaskTypePipe implements PipeTransform {
  transform(value: TaskType | undefined): string {
    switch (value) {    
      case TaskType.Deployment:
        return 'Wdrożenie';
      case TaskType.Maintenance:
        return 'Maintanance';
      case TaskType.Implementation:
        return 'Implementacja';
      case undefined:
      default:
        return 'Unknown Task type';
    }
  }
}

@Pipe({
  name: 'status'
})
export class StatusPipe implements PipeTransform {
  transform(value: Status | undefined): string {
    switch (value ) {
      case Status.Finished:
        return 'Finished';
      case Status.Unfinished:
        return 'Unfinished';
      case undefined:
      default:
        return 'Unknown Status';
    }
  }
}
