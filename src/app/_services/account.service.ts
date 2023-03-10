import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl="http://localhost:5193/api/";
  private currentUserSource=new ReplaySubject<User>(1);
  currentUser$=this.currentUserSource.asObservable();
  constructor(private http: HttpClient) { }
  login(model: any)//dữ liệu model là thông tin do người dùng nhập vào thuộc kiểu dữ liệu bất kì
  {
    return this.http.post<User>(this.baseUrl+'account/login',model).pipe(
      map((response : User)=>{
        const user=response;//phương thức map nhận đối tượng reponse từ serve để gán cho 'user' 
        if(user){
          localStorage.setItem('user',JSON.stringify(user));//lưu trữ đối tượng 'user' dưới dạng JSON vào localStrorage
          this.currentUserSource.next(user);//thông báo cho ứng dụng 1 người mới đã đăng nhập
        }
      })
    )
  }
  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/register',model).pipe(
      map((user : User)=>{
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
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
