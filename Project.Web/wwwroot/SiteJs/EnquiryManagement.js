$(document).ready(function () {
    BindEnquiries();
    
});

/**
 * Bind Enquiries List.
 */
function BindEnquiries() {
    $('#Enquiries_tbl').DataTable({
        "processing": true, // for show progress bar    
        "serverSide": true, // for process server side    
        "filter": true, // this is for disable filter (search box)    
        "orderMulti": false, // for disable multiple column at once 
        "pageLength": 10,
        "language": {
            "zeroRecords": "Nothing found - sorry",
            "infoEmpty": "No records available",
            "infoFiltered": "",
            paginate: {
                next: '<i class="fa fa-angle-right"></i>',
                previous: '<i class="fa fa-angle-left"></i>'
            }
        },
        responsive: true,
        "order": [[4, "desc"]],
        "ajax": {
            url: window.appBase + "admin/EnquiryManagement/GetEnquiries",
            type: "POST",
            datatype: "json",
            complete: function (xhr, responseText) {
                console.log(xhr);
                console.log(responseText); //*** responseJSON: Array[0]
            }
        },
        "columnDefs": [
        ],
        "columns": [
            { "data": "firstName", "name": "FirstName", "autoWidth": true },
            { "data": "lastName", "name": "LastName", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "phoneNo", "name": "PhoneNumber", "autoWidth": true },
            { "data": "country", "name": "Country", "autoWidth": true },
            { "data": "createdOn", "name": "Date", "autoWidth": true },
            { "data": "message", "name": "Message", "autoWidth": true }
        ],
        "initComplete": function (settings, json) {

        }

    });
}