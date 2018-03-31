import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {
  MatToolbarModule,
  MatSidenavModule,
  MatButtonModule,
  MatIconModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatInputModule,
  MatCheckboxModule,
  MatTableModule
} from '@angular/material';

import { SignalrService } from './signalr.service';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SideContentComponent } from './side-content/side-content.component';
import { SideBarComponent } from './side-content/side-bar/side-bar.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SideContentComponent,
    SideBarComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatCheckboxModule,
    MatTableModule
  ],
  providers: [
    SignalrService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
