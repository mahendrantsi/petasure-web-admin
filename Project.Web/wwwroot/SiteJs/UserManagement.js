$(document).ready(function () {
    BindUsers();

});

/**
 * Bind User List.
 */
function BindUsers() {
    $('#user_tbl').DataTable({
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
        "order": [[3, "desc"]],
        "ajax": {
            url: "/admin/UserManagement/GetUsers",
            type: "POST",
            datatype: "json",
            complete: function (xhr, responseText) {
                console.log(xhr);
                console.log(responseText); //*** responseJSON: Array[0]
            }
        },
        "columnDefs": [
            { targets: [4], orderable: false }
        ],
        "columns": [
            { "data": "userName", "name": "UserName", "autoWidth": true,  },
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": true },
            { "data": "strCreatedOn", "name": "StrCreatedOn", "autoWidth": true },
            {
                data: null,
                render: function (data, type, row, meta) {
                    var link = '<div class="d-flex align-items-center flex-nowrap">';
                    link += "<a class='icon-wrap-box' href='/admin/UserManagement/EditUser/" + row.enc_Id + "'><i class='fa fa-edit'></i></a> ";
                    link += "</div>"
                    return link;
                }
            }

        ]
    });
}