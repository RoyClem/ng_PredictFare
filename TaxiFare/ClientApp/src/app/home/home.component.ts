import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:  ['./home.component.css']
})
export class HomeComponent {

  public vendorIds: string[];
  public rateCodes: string[];
  public payTypes: string[];
  public numPassengers: string[];

  public vendorId: string = "CMT" ;
  public rateCode: string = "0";
  public payType: string = "CRD";
  public numPass: string = "1";
  public tripDis: string = "4";

  public showResult: boolean = false;
  public result : string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get(baseUrl + 'api/SampleData/LoadData')
      .subscribe(result => {
      this.vendorIds = result['vendors'];
      this.rateCodes = result['rates'];
      this.payTypes = result['payTypes'];
      this.numPassengers = result['numPass'];
    }, error => console.error(error));
  }

  public getPrediction() {

    this.http.post(this.baseUrl + '/api/SampleData/GetPrediction', {
      vendorId: this.vendorId,
      rateCode: this.rateCode,
      passengerCount: this.numPass,
      tripDistance: this.tripDis,
      paymentType: this.payType
    })
      .subscribe(
      result => {
        this.showResult = true;
        this.result = "";
        this.result += "Predicted Fare Amount is: " + result;
        console.log('success', result)
      },
      error => console.log('Error', error)
    );

  }
}
