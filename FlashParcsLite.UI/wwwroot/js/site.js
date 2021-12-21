$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:17648/parkingHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();
    var apiUrl = "https://localhost:17648/api/ParkingLocation";
    var addVehicleButton = $("#add-vehicle");
    var removeVehicleButton = $("#remove-vehicle");
    var vehicleCount = $("#vehicle-count");
    var parkingLocation;

    async function start() {
        try {
            if (connection.state === signalR.HubConnectionState.Disconnected) {
                await connection.start();
            }            
            getSelectedParkingLocation();
            checkButtonStatus();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    start();

    function checkButtonStatus() {

        if (parkingLocation.vehicleCount == 0) {
            removeVehicleButton.prop("disabled", true);
            addVehicleButton.prop("disabled", false);
        }
        else if (parkingLocation.vehicleCount == parkingLocation.capacity) {
            removeVehicleButton.prop("disabled", false);
            addVehicleButton.prop("disabled", true);
        }
        else {
            addVehicleButton.prop("disabled", false);
            removeVehicleButton.prop("disabled", false);
        }
    }

    function getSelectedParkingLocation() {
        var selectedOptionValue = $("#locations option:selected").val();
        var jqxhr = $.get(`${apiUrl}/${selectedOptionValue}`)
            .done(function (data) {
                parkingLocation = data;
                vehicleCount.text(parkingLocation.vehicleCount);
            })
            .fail(function () {
                alert("error");
            });
    }

    connection.on("ReceiveLocationInfo", function (pl) {
        parkingLocation = pl;
        vehicleCount.text(parkingLocation.vehicleCount);
        checkButtonStatus();
    })

    $("#locations").on("change", function (event) {
        getSelectedParkingLocation();
        checkButtonStatus();
    });

    addVehicleButton.on("click", function (event) {
        $.ajax({
            type: "POST",
            url: `${apiUrl}/updateVehicleCount`,
            data: JSON.stringify({ parkingLocationId: parseInt($("#locations option:selected").val()), updateVehicleCount: "increase" }),
            contentType: 'application/json',
            success: function () {
                console.log("increased vehicle count");
                checkButtonStatus();
            },
            fail: function () {
                alert("unable to add vehicle to the location");
            },
            dataType: 'json'
        });             
    });

    removeVehicleButton.on("click", function (event) {
        $.ajax({
            type: "POST",
            url: `${apiUrl}/updateVehicleCount`,
            data: JSON.stringify({ parkingLocationId: parseInt($("#locations option:selected").val()), updateVehicleCount: "decrease" }),
            contentType: 'application/json',
            success: function () {
                console.log("decrease vehicle count");
                checkButtonStatus();
            },
            fail: function () {
                alert("unable to remove vehicle to the location");
            },
            dataType: 'json'
        });           
    });
});