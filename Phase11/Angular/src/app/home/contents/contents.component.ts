import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-contents',
  templateUrl: './contents.component.html',
  styleUrls: ['./contents.component.scss']
})
export class ContentsComponent implements OnInit {
  public results: string[] = [];
  public resultsStatus: string;

  constructor() {
    this.resultsStatus = 'init';
  }

  ngOnInit(): void {
  }

  public resultsHandler(results: string[]): void {
    this.results = results;
    if (this.results.length > 0) {
      this.resultsStatus = 'found';
    } else {
      this.resultsStatus = 'not-found';
    }
  }
}
