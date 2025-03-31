export interface PagedData<T> {
  totalItems: number; 
  pageIndex: number;  
  pageSize: number;
  statistics: Record<string,any> | null,
  items: T[];         
}
