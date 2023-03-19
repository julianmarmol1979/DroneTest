
import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-testdrone-component',
  templateUrl: './testdrone.component.html'
})

export class TestDroneComponent {

  textInput!: string;
  textOutput!: string;


  public droneresult: string[] = [];
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { } 

  testDrone() {
    //Passing parameters
    let params: HttpParams = new HttpParams();
    params = params.append('input', this.textInput);

    let httpOptions = {
      params: params
    };

    this.http.get<string[]>(this.baseUrl + 'drone', httpOptions).subscribe(result => {
      this.droneresult = result;
    }, error => console.error(error));;
   
  }
}
 




