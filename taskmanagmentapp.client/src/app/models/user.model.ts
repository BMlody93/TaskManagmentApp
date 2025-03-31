export interface User {
  id: number;
  username: string;
  userType: UserType;
}

export enum UserType {
  Admin = 0,
  Programmer = 1
}
