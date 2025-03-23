// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// SignalR connection

let connection = null;

// Initialize SignalR
function initializeSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/messageHub")
        .withAutomaticReconnect()
        .build();

    connection.start()
        .then(function () {
            console.log("SignalR Connected.");
        })
        .catch(function (err) {
            console.error(err.toString());
        });

    connection.on("ReceiveBookingStatusUpdate", function () {
        if (location.pathname.includes("/Booking")) {
            location.reload();
        }
    });

    connection.on("ReceiveCampusUpdate", function () {
        if (location.pathname.includes("/Campus")) {
            location.reload();
        }
    });

    connection.on("ReceiveDepartmentUpdate", function () {
        if (location.pathname.includes("/Department")) {
            location.reload();
        }
    });

    connection.on("ReceiveRoomUpdate", function () {
        if (location.pathname.includes("/Room")) {
            location.reload();
        }
    });
}
