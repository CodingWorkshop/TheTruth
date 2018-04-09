import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppMaterialModule } from './app.material.module';

import { SignalrService } from './signalr.service';
import { VideoService } from './video.service';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { WrapperComponent } from './wrapper/wrapper.component';
import { SideContentComponent } from './side-content/side-content.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { SetupDialogComponent } from './setup-dialog/setup-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    WrapperComponent,
    SideContentComponent,
    SideBarComponent,
    SetupDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppMaterialModule
  ],
  entryComponents: [
    WrapperComponent,
    SetupDialogComponent
  ],
  providers: [
    SignalrService,
    VideoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
