import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {BackGroundImgComponent} from './back-ground-img/back-ground-img.component';
import {ContentsComponent} from './contents/contents.component';
import {FormsModule} from '@angular/forms';
import {SearchBoxComponent} from './contents/search-box/search-box.component';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {ResultsComponent} from './contents/results/results.component';
import {NormalSearchComponent} from './contents/search-box/normal-search/normal-search.component';
import {AdvanceSearchComponent} from './contents/search-box/advance-search/advance-search.component';
import {HttpService} from './contents/services/http.service';
import {FileContentsComponent} from './file-contents/file-contents.component';
import {RouterModule} from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes = [
  {path: 'home', component: AppComponent},
  {path: 'file/:fileName', component: FileContentsComponent}
];


@NgModule({
  declarations: [
    AppComponent,
    BackGroundImgComponent,
    ContentsComponent,
    SearchBoxComponent,
    ResultsComponent,
    NormalSearchComponent,
    AdvanceSearchComponent,
    FileContentsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ],
  providers: [HttpService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule {
}
