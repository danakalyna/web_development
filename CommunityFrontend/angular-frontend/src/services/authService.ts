import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(
        JSON.parse(localStorage.getItem('currentUser') || '{}')
      );      
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  public get currentUserId() {
    return this.currentUserSubject.value.userId;
  }  
  
  public get currentUsername() {
    return this.currentUserSubject.value.username;
  }

  login(username: string, password: string) {
    return this.http.post<any>(`${environment.apiUrl}api/auth/login`, { username, password })
      .pipe(map(user => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  convertDate(dateString: string) {
    
    const time = "16:57:11.646";
    const datetime = new Date(`${dateString}T${time}Z`);
    return datetime.toISOString(); // Converts to a string in simplified extended ISO format (ISO 8601)
  }

  register(firstName: string, lastName: string, email: string, password: string, dateOfBirth: string, isMember: boolean) {
    return this.http.post<any>(`${environment.apiUrl}Users`, { 
      firstName, 
      lastName, 
      email, 
      passwordHash: password, 
      dateOfBirth: this.convertDate(dateOfBirth), 
      isMember,
      userPermissions: [],
      activities: [],
      bookings: []
    }).pipe(map(user => {
      // Optionally store user details in local storage or handle as needed
      return user;
    }));
  }
}
