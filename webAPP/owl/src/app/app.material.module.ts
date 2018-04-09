import { NgModule } from '@angular/core';
import {
  MatToolbarModule,
  MatSidenavModule,
  MatButtonModule,
  MatIconModule,
  MatDatepickerModule,
  MatInputModule,
  MatCheckboxModule,
  MatTableModule,
  MatCardModule,
  MatDialogModule,
  MatGridListModule,
  MatProgressSpinnerModule
} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

@NgModule({
  exports: [
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
    MatDialogModule,
    MatGridListModule,
    MatProgressSpinnerModule
  ]
})
export class AppMaterialModule { }
