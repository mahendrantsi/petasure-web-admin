$(document).ready(function () {
    BindMenus();

});

/**
 * Bind Menu List.
 */
function BindMenus() {
    $('#menu_tbl').DataTable({
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
            url: window.appBase + "admin/Menu/GetMenuResult",
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
            { "data": "menuName", "name": "MenuName", "autoWidth": true },
            { "data": "displayName", "name": "DisplayName", "autoWidth": true },
            { "data": "parentMenu", "name": "ParentMenu", "autoWidth": true },           
            { "data": "url", "name": "Url", "autoWidth": true },
            {
                data: "IsActive",
                render: function (data, type) {
                    if (data) {
                        return "<input type='checkbox' disabled checked >";
                    }
                    else {
                        return "<input type='checkbox' disabled >";
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row, meta) {
                    var link = '<div class="d-flex align-items-center flex-nowrap">';
                    link += "<a class='icon-wrap-box' href='" + window.appBase + "admin/Menu/Edit/" + row.enc_Id + "'><i class='fa fa-edit'></i></a> ";
                    link += "<a class='icon-wrap-box' href='" + window.appBase + "admin/Menu/Detail/" + row.enc_Id + "'><i class='fa fa-eye'></i></a> ";
                    link += "</div>"
                    return link;
                }
            }

        ],
        "initComplete": function (settings, json) {

        }

    });
}