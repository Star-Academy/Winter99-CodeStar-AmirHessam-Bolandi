import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {HttpClient, HttpClientModule} from '@angular/common/http';

import {AppComponent} from './app.component';
import {BackGroundImgComponent} from './back-ground-img/back-ground-img.component';
import {MenuBarComponent} from './menu-bar/menu-bar.component';
import {HomeComponent} from './home/home.component';
import {ContentsComponent} from './home/contents/contents.component';
import {SearchBoxComponent} from './home/contents/search-box/search-box.component';
import {ResultsComponent} from './home/contents/results/results.component';
import {NormalSearchComponent} from './home/contents/search-box/normal-search/normal-search.component';
import {AdvanceSearchComponent} from './home/contents/search-box/advance-search/advance-search.component';
import {FileContentsComponent} from './file-contents/file-contents.component';

import {HttpService} from './services/http.service';

const routes = [
  {path: 'home', component: HomeComponent},
  {path: '', redirect: 'home', component: HomeComponent},
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
    HomeComponent,
    MenuBarComponent
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
