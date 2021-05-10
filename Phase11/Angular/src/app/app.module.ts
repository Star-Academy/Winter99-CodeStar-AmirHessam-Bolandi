import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {BackGroundImgComponent} from './back-ground-img/back-ground-img.component';
import {ContentsComponent} from './contents/contents.component';
import {FormsModule} from '@angular/forms';
import {SearchBoxComponent} from './contents/search-box/search-box.component';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import { ResultsComponent } from './contents/results/results.component';
import { NormalSearchComponent } from './contents/search-box/normal-search/normal-search.component';
import { AdvanceSearchComponent } from './contents/search-box/advance-search/advance-search.component';
import {HttpService} from './contents/services/http.service';

@NgModule({
  declarations: [
    AppComponent,
    BackGroundImgComponent,
    ContentsComponent,
    SearchBoxComponent,
    ResultsComponent,
    NormalSearchComponent,
    AdvanceSearchComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [HttpService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule {
}
