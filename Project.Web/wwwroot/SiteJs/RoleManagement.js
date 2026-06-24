$(document).ready(function(){
    $('#role_tbl').DataTable({
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
        "order": [[1, "asc"]],
        "ajax": {
            url: "/admin/UserRoleManagement/GetRoleResult",
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
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "normalizedName", "name": "NormalizedName", "autoWidth": true },
            {
                data: null,
                render: function (data, type, row, meta) {
                    var link = '<div class="d-flex align-items-center flex-nowrap">';
                    link += "<a class='icon-wrap-box' href='/admin/UserRoleManagement/EditRole/" + row.enc_Id + "'><i class='fa fa-edit'></i></a> ";
                    link += "<a class='icon-wrap-box' href='./ManageAccess/" + row.enc_Id + "'>Manage Access</a> ";
                    link += "</div>"
                    return link;
                }
            }

        ],
        "initComplete": function (settings, json) {

        }

    });
});