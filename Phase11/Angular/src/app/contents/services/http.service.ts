import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Document} from '../../file-contents/models/Document';

@Injectable()
export class HttpService {
  private url = 'https://localhost:5001/';

  constructor(private httpClient: HttpClient) {
  }

  public async getQueryResult(normQuery: string = '', plusQuery: string = '', minusQuery: string = ''): Promise<string> {
    if (normQuery + plusQuery + minusQuery === '') {
      return;
    }
    const parameters = [];
    if (normQuery !== '') {
      parameters.push('normals="' + normQuery + '"');
    }
    if (minusQuery !== '') {
      parameters.push('minuses="' + minusQuery + '"');
    }
    if (plusQuery !== '') {
      parameters.push('pluses="' + plusQuery + '"');
    }
    const queryString = 'query?' + parameters.join('&');
    return this.httpClient.get(this.url + queryString, {responseType: 'text'}).toPromise();
  }

  public async getFile(fileName: string): Promise<string> {
    return this.httpClient.get(this.url + 'file/' + fileName, {responseType: 'text'}).toPromise();
  }

  public async initElastic(): Promise<string> {
    return this.httpClient.put<string>(this.url + 'init/yes', '').toPromise();
  }

}
