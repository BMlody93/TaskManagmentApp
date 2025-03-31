import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CacheService {


  private cache: { [key: string]: any } = {};

  constructor() { }

  // Get data from the cache
  get<T>(key: string): T | null {
    return this.cache[key] ?? null; 
  }

  // Set data in the cache
  set(key: string, value: any): void {
    this.cache[key] = value;
  }

  // Clear a specific item in the cache
  remove(key: string): void {
    delete this.cache[key];
  }

  // Clear all cache data
  clear(): void {
    this.cache = {};
  }
}
