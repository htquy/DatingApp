import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import{ environment } from"src/environments/environment";
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl=environment.apiUrl;
  private currentUserSource=new ReplaySubject<User>(1);
  currentUser$=this.currentUserSource.asObservable();
  constructor(private http: HttpClient) { }
  login(model: any)//dữ liệu model là thông tin do người dùng nhập vào thuộc kiểu dữ liệu bất kì
  {
    return this.http.post<User>(this.baseUrl+'account/login',model).pipe(
      map((response : User)=>{
        const user=response;//phương thức map nhận đối tượng reponse từ serve để gán cho 'user' 
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }
  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/register',model).pipe(
      map((user : User)=>{
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }
  setCurrentUser(user:User){
    this.currentUserSource.next(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null!);
  }
}
