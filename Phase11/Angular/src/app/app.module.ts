import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {BackGroundImgComponent} from './back-ground-img/back-ground-img.component';
import {ContentsComponent} from './home/contents/contents.component';
import {FormsModule} from '@angular/forms';
import {SearchBoxComponent} from './home/contents/search-box/search-box.component';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {ResultsComponent} from './home/contents/results/results.component';
import {NormalSearchComponent} from './home/contents/search-box/normal-search/normal-search.component';
import {AdvanceSearchComponent} from './home/contents/search-box/advance-search/advance-search.component';
import {HttpService} from './services/http.service';
import {FileContentsComponent} from './file-contents/file-contents.component';
import {RouterModule} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MenuBarComponent } from './menu-bar/menu-bar.component';

const routes = [
  {path: 'home', component: HomeComponent},
  {path: '', redirect: 'home' , component: HomeComponent},
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
