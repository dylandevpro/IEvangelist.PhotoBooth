import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CameraComponent } from './camera/camera.component';
import { ControlWizardComponent } from './control-wizard/control-wizard.component';

import { ImageService } from './services/image.service';
import { NumberPadComponent } from './number-pad/number-pad.component';
import { PhoneNumberPipe } from './pipes/phone-number.pipe';
import { AudioComponent } from './audio/audio.component';
import { SharedComponent } from './shared/shared.component';
import { ShareModule } from '@ngx-share/core';
import { environment } from '../environments/environment';

@NgModule({
    declarations: [
        AppComponent,
        CameraComponent,
        ControlWizardComponent,
        NumberPadComponent,
        PhoneNumberPipe,
        AudioComponent,
        SharedComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        ShareModule.forRoot(),
        RouterModule.forRoot([
            { path: '', component: CameraComponent, pathMatch: 'full' },
            { path: 'images/:id', component: SharedComponent }
        ])
    ],
    providers: [
        ImageService,
        { provide: 'BASE_API_URL', useValue: environment.baseApiUrl },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
