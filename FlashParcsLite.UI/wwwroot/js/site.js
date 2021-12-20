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
            await connection.start();
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

    connection.on("ReceiveLocationInfo", function (pl) {
        parkingLocation = pl;
        vehicleCount.text(parkingLocation.vehicleCount);
    })

    $("#locations").on("change", function (event) {
        var selectedOptionValue = $("#locations option:selected").val();

        var jqxhr = $.get(`${apiUrl}/${selectedOptionValue}`)
            .done(function (data) {
                var location = data;
                console.log(location);
                vehicleCount.text(location.vehicleCount);
            })
            .fail(function () {
                alert("error");
            });
    });

    addVehicleButton.on("click", function (event) {
        $.ajax({
            type: "POST",
            url: `${apiUrl}/updateVehicleCount`,
            data: JSON.stringify({ parkingLocationId: parseInt($("#locations option:selected").val()), updateVehicleCount: "increase" }),
            contentType: 'application/json',
            success: function () {
                console.log("increased vehicle count");
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
            },
            dataType: 'json'
        });           
    });
});