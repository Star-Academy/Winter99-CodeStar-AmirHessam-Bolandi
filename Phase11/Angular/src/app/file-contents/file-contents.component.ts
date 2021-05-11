import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpService} from '../services/http.service';
import {Document} from './models/Document';

@Component({
  selector: 'app-file-contents',
  templateUrl: './file-contents.component.html',
  styleUrls: ['./file-contents.component.scss']
})
export class FileContentsComponent implements OnInit {
  public document: Document;

  constructor(private route: ActivatedRoute,
              private router: Router, private httpService: HttpService) {
    const fileName = route.snapshot.paramMap.get('fileName');
    this.getFileContent(fileName);
  }

  ngOnInit(): void {
  }

  public async getFileContent(fileName: string): Promise<void> {
    const response = JSON.parse(await this.httpService.getFile(fileName));
    if (response.length > 0) {
      this.document = new Document(response[0].DocumentId, response[0].Content);
    } else {
      this.document = new Document('File Not Found', '');
    }
  }

}
