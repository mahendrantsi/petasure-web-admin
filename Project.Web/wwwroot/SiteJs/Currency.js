$(document).ready(function () {
    BindCurrencies();

});

/**
 * Bind Currencies.
 */
function BindCurrencies() {
    $('#currency_tbl').DataTable({
        "processing": true, // for show progress bar    
        "serverSide": true, // for process server side    
        "filter": true, // this is for disable filter (search box)    
        "orderMulti": false, // for disable multiple column at once 
        "pageLength": 10,
        "language": {
            "zeroRecords": "No data found.",
            "infoEmpty": "No data found.",
            "infoFiltered": "",
            paginate: {
                next: '<i class="fa fa-angle-right"></i>',
                previous: '<i class="fa fa-angle-left"></i>'
            }
        },
        responsive: true,
        "order": [[5, "desc"]],
        "ajax": {
            url: window.appBase + "admin/Currency/GetCurrencies",
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
            { "data": "description", "name": "Description", "autoWidth": true },
            {
                "data": "isActive", "name": "IsActive", "autoWidth": true,
                render: function (data, type, row, meta) {
                    return row.isActive === true ? '<i class="fa fa-thumbs-up"></i>' : '<i class="fa fa-thumbs-down"></i>';
                }
            },
            {
                "data": "isBaseCurrency", "name": "IsBaseCurrency", "autoWidth": true,
                render: function (data, type, row, meta) {
                    return row.isBaseCurrency === true ? '<i class="fa fa-thumbs-up"></i>' : '<i class="fa fa-thumbs-down"></i>' ;
                } },
            {
                data: "createdOn",
                name: "CreatedOn",
                autoWidth: true,
                render: function (data, type, row, meta) {
                    return moment(row.createdOn).format("DD-MM-YYYY HH:MM:SS");
                }
            },
            {
                data: null,
                render: function (data, type, row, meta) {
                    var link = '<div class="d-flex align-items-center flex-nowrap">';
                    link += "<a class='icon-wrap-box' href='" + window.appBase + "Admin/Currency/Edit?id=" + row.enc_Id + "' data-toggle='tooltip' title='Edit'><i class='fa fa-edit'></i></a>&nbsp;";
                    link += "</div>"
                    return link;
                }
            }
        ],
        "initComplete": function (settings, json) {

        }

    });
}