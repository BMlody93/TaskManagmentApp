import { bootstrapApplication } from '@angular/platform-browser';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { provideAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { AppComponent } from './app/app.component';
import { LoadingInterceptor } from './app/interceptors/loading.interceptor';
import { ErrorInterceptor } from './app/interceptors/error.interceptor';


bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptors([LoadingInterceptor, ErrorInterceptor])),
    provideAnimations(),
    importProvidersFrom(MatSnackBarModule)
  ],
});
