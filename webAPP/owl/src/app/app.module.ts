import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {
  MatToolbarModule,
  MatSidenavModule,
  MatButtonModule,
  MatIconModule,
  MatDatepickerModule,
  MatInputModule,
  MatCheckboxModule,
  MatTableModule,
  MatCardModule
} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

import { SignalrService } from './signalr.service';
import { VideoService } from './video.service';

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
    MatMomentDateModule,
    MatInputModule,
    MatCheckboxModule,
    MatTableModule,
    MatCardModule,
    HttpClientModule
  ],
  providers: [
    SignalrService,
    VideoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
