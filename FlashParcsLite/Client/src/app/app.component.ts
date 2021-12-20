import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Location } from '@angular/common';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent {
  public locations: ParkingLocation[];
  //public vehicleCount: number;
  public selectedLocation: ParkingLocation;
  private connection: signalR.HubConnection;

  constructor(http: HttpClient) {

    var baseUrl = environment.BASE_URL;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/parkingHub')
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.start();

    http.get<ParkingLocation[]>(baseUrl + '/location').subscribe(result => {
      this.locations = result;
    }, error => console.error(error));

    this.connection.on("ReceiveLocationInfo", this.receiveLocationInfo);

    this.selectedLocation = {
      id: 0,
      name: 'unknown location',
      vehicleCount: 0,
      capacity: 0
    }

    this.locations = [
      this.selectedLocation
    ];

    //this.vehicleCount = this.selectedLocation.vehicleCount;
  }

  private start = async() => {
    try {
      await this.connection.start();
      console.log("connected");
    } catch (err) {
      console.log(err);
      setTimeout(() => this.start(), 5000);
    }
  };

  private receiveLocationInfo = (pl: any) => {
    this.selectedLocation = pl;
  }

  public onLocationChange = () => {
    this.connection.invoke("GetLocationAsync", this.selectedLocation.id).catch(function (err) {
      return console.error(err.toString());
    })
  }

  public increaseVehicleCount = () => {
    this.connection.invoke("IncreaseVehicleCount", this.selectedLocation.id).catch(function (err) {
      return console.error(err.toString());
    })
  }

  public decreaseVehicleCount = () => {
    this.connection.invoke("DecreaseVehicleCount", this.selectedLocation.id).catch(function (err) {
      return console.error(err.toString());
    })
  }
}

interface ParkingLocation {
  id: number;
  name: string;
  vehicleCount: number;
  capacity: number;
}
