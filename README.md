<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#installation">Installation</a>      
    </li>
    <li>
      <a href="#inner-workings">Inner Workings</a>      
    </li> 
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

# FlashParcsLite
Portal to View (realtime), Add and Remove Vehicles from different parking locations


### Built With
* [SignlR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-3.1)
* [Bootstrap](https://getbootstrap.com)
* [JQuery](https://jquery.com)

### Installation
1. Clone the repo
   ```sh
   git clone https://github.com/parthchopra/FlashParcsLite.git
   ```
2. Set `FlashParcsLite.API` and `FlashParcsLite.UI` as multiple startup projects in the solution
3. On the first run of the app, it will create a sqlite database in the local folder and seed some initial data to test the app

### Inner Workings
1. Each parking location has a Name, ID, Current Vehicle Count and Capacity
2. `FlashParcsLite.Data` has the models, repository and data context and validation logic to ensure clean data
3. `FlashParcsLite.API` serves the data from the sqlite database and has the following methods
    - Get All Parking Locations
    - Get Details of a single parking location
    - Post Update to Parking location to increase or decrease the vehicle count
    - In addition to the above REST methods the API also has SignalR Hub to broadcast updates in vehicle count to all the clients
4. `FlashParcsLite.UI` is the web front end of the application
    - It has service to fetch data from the API and serve the HTML on the web page
    - It has a drop down to select a parking location and has buttons to increase or decrease vehicle count on a location
    - Javascript makes Ajax calls to the API controller to Get or Post updates to the parking location.
    - In addition to the Ajax calls, there is a signalR client which receives updated vehicle count in the parking location.
    - Based on the vehicle count, the UI will enable or disable the button to Add or Remove Vehicle from a location.
5. Solution includes two test projects
    - Unit test project extensively uses mocks to mock out dependencies
    - Integration tests project uses an in memory data store to test the logic in repository
