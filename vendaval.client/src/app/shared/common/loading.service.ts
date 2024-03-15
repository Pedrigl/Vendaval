import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  public static isLoading = new BehaviorSubject<boolean>(false);
  constructor() { }
}
