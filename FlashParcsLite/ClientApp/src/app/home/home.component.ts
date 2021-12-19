import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
    public locations: Location[];
    public vehicleCount: number;
    public selectedLocation: Location;
    private connection: signalR.HubConnection;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

        this.connection = new signalR.HubConnectionBuilder().withUrl("/parkingHub").build();
        this.connection.start().then(function () {
        }).catch(function (err) {
            return console.error(err.toString());
        });

        http.get<Location[]>(baseUrl + 'location').subscribe(result => {
            this.locations = result;
        }, error => console.error(error));

        this.connection.on("ReceiveLocationInfo", this.receiveLocationInfo);

        this.vehicleCount = 0;
    }

    private receiveLocationInfo = (pl) => {
        this.vehicleCount = pl.vehicleCount;
    }

    public onLocationChange = () => {
        this.connection.invoke("GetLocationAsync", this.selectedLocation.id).catch(function (err) {
            return console.error(err.toString());
        })
    }
}

interface Location {
    id: number;
    name: string;
    vehicleCount: number;
    capacity: number;
}