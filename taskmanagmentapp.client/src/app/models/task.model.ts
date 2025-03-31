import { PagedData } from './paged-data.model';

export interface Task {
  id: number;
  assignedUserId: number | null;
  title: string;
  text: string;
  difficulty: number;
  taskType: TaskType;
  details: Record<string, any>;
}

export enum Status {
  Finished = 1,
  Unfinished = 0
}

export enum TaskType {
  Deployment = 0,
  Maintenance = 1,
  Implementation = 2
}

export interface PagedTaskData extends PagedData<Task> { }
