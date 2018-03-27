import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatToolbarModule } from '@angular/material';
import { MatSidenavModule } from '@angular/material/sidenav';

import { SignalrService } from './signalr.service';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SideContentComponent } from './side-content/side-content.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SideContentComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule
  ],
  providers: [
    SignalrService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
