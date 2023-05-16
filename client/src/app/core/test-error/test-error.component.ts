import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent {
  constructor (private http: HttpClient) {}

  baseUrl = environment.apiUrl;

  get404Error() {
    this.http.get(this.baseUrl + 'product/42').subscribe(
      response => {console.log(response);},
      error => {console.log(error)}
      )
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe(
      response => {console.log(response);},
      error => {console.log(error)}
      )
  }

  get400ValidationError() {
    this.http.get(this.baseUrl + 'product/aaaa').subscribe(
      response => {console.log(response);},
      error => {console.log(error)}
      )
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(
      response => {console.log(response);},
      error => {console.log(error)}
      )
  }

}
