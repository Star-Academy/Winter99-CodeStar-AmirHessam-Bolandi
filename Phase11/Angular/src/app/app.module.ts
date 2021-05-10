import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {BackGroundImgComponent} from './back-ground-img/back-ground-img.component';
import {ContentsComponent} from './contents/contents.component';
import {FormsModule} from '@angular/forms';
import {SearchBoxComponent} from './contents/search-box/search-box.component';
import {ClosedResultBoxComponent} from './contents/closed-result-box/closed-result-box.component';
import {ContentsService} from './contents/services/contents.service';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import { ResultsComponent } from './contents/results/results.component';

@NgModule({
  declarations: [
    AppComponent,
    BackGroundImgComponent,
    ContentsComponent,
    SearchBoxComponent,
    ClosedResultBoxComponent,
    ResultsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [ContentsService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule {
}
