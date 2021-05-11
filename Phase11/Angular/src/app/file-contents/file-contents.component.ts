import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-file-contents',
  templateUrl: './file-contents.component.html',
  styleUrls: ['./file-contents.component.scss']
})
export class FileContentsComponent implements OnInit {

  constructor(private route: ActivatedRoute,
              private router: Router) {
    console.log(this.route.toString());
    console.log(route.params);
    console.log(this.router.toString());
  }

  ngOnInit(): void {
    console.log(this.route.toString());
    console.log(this.route.params);
    console.log(this.router.toString());
  }

}
