import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-contents',
  templateUrl: './contents.component.html',
  styleUrls: ['./contents.component.scss']
})
export class ContentsComponent implements OnInit {
  public results: string[] = ['ali', 'res'];
  public resultsStatus: string;
  constructor() {
    this.resultsStatus = 'init';
  }

  ngOnInit(): void {
  }

}
