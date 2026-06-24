import { signalR } from "../microsoft/signalr/dist/browser/signalr";

$(() => {
    debugger;
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadTradeData", function () {
        alert();
        //LoadTradeData();
    });

    //function LoadTradeData() {
    //    $.ajax({
    //        url: "/customer/Trade/",
    //        type: "GET",
    //        success: (result) => {
    //            debugger;
    //            $.each(result, (k, v) => {

    //            });
    //        }

    //    });
    //}
});
