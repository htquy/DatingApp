import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],// cai dat cac component,pipe,dirctive cho chuong trinh
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],// tap hop cac dodule co kha nang cung cap cho chuong trinh
  providers: [],//cung cap module(component) de su dung cho chuong trinh khac
  bootstrap: [AppComponent]
})
export class AppModule { }
