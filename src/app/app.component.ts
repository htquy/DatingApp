import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',//kich hoat chay code trong mau "app-root"
  templateUrl: './app.component.html',//duong dan URL tuyet doi
  styleUrls: ['./app.component.css']//duong dan cung cap css
})
export class AppComponent implements OnInit{
  title = 'The Dating App';
  users:any;
  constructor(private http:HttpClient){}
  ngOnInit() {
    this.getUsers();
  }
  getUsers(){
    this.http.get('http://localhost:5193/api/users').subscribe(response=>{
      this.users=response;
    },error =>{console.log(error)}
    );//log ra ket qua du lieu tu API qua phuong thuc subscribe
  }
}
