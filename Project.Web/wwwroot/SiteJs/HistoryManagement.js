function ShowHistoryModal(obj) {
    
    var id = $(obj).attr('UserId');
    $.get("/Admin/HistoryManagement/GetUserHistory", { Id: id }, function (res) {
        debugger
        //  alert(res)
        $('#UserHistory .modal-body').html(res);
        $('#UserHistory').modal('show')

    });
}

//$(document).ready(function () {
//    BindTransactions();

//});

/**
 * Bind Transactions List.
 */
//function BindTransactions() {
//    debugger
//    $('#UserHistory_tbl').DataTable({
//        "processing": true, // for show progress bar    
//        "serverSide": true, // for process server side    
//        "filter": true, // this is for disable filter (search box)    
//        "orderMulti": false, // for disable multiple column at once 
//        "pageLength": 10,
//        "language": {
//            "zeroRecords": "Nothing found - sorry",
//            "infoEmpty": "No records available",
//            "infoFiltered": "",
//            paginate: {
//                next: '<i class="fa fa-angle-right"></i>',
//                previous: '<i class="fa fa-angle-left"></i>'
//            }
//        },
//        responsive: true,
//        "order": [[3, "desc"]],
//        "ajax": {
//            url: "/User/HistoryManagement/GetUserHistory",
//            type: "POST",
//            datatype: "json",
//            complete: function (xhr, responseText) {
//                console.log(xhr);
//                console.log(responseText); //*** responseJSON: Array[0]
//            }
//        },
//        "columnDefs": [
//        ],
//        "columns": [
//            { "data": "userName", "name": "userName", "autoWidth": true },
//            { "data": "email", "name": "email", "autoWidth": true },
//            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
//            { "data": "isKyc", "name": "isKyc", "autoWidth": true }
//            //{ "data": "amount", "name": "Amount", "autoWidth": true }
//        ],
//        "initComplete": function (settings, json) {

//        }

//    });
//}