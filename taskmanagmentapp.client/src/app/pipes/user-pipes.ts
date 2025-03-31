import { Pipe, PipeTransform } from '@angular/core';
import { UserType } from '../models/user.model';

@Pipe({
  name: 'userType'
})
export class UserTypePipe implements PipeTransform {
  transform(value: UserType): string {
    switch (value) {
      case UserType.Admin:
        return 'DevOps/Administrator';
      case UserType.Programmer:
        return 'Programista';
      default:
        return 'Unknown Task';
    }
  }
}
