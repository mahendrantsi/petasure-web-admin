$(document).ready(function () {
        BindFeeDataResult();

});
/**
 * Bind Fee List.
 */
function BindFeeDataResult() {
    $('#fee_tbl').DataTable({
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
        "order": [[5, "desc"]],
        "ajax": {
            url: "/admin/Fee/GetFeeListDataResult",
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
            { "data": "transactionTypeName", "name": "TransactionTypeName", "autoWidth": true },
            { "data": "feeType", "name": "FeeType", "autoWidth": true },
            { "data": "defaultFeeAmount", "name": "DefaultFeeAmount", "autoWidth": true },
            { "data": "strCreatedOn", "name": "StrCreatedOn", "autoWidth": true },
            {
                data: null,
                render: function (data, type, row, meta) {

                    var link = '<div class="d-flex align-items-center flex-nowrap">';
                    link += "<a class='icon-wrap-box' href='/admin/fee/Edit/" + row.enc_Id + "' data-toggle='tooltip' title='Edit'><i class='fa fa-edit'></i></a> ";
                    link += "<a class='icon-wrap-box' href='/admin/fee/Detail/" + row.enc_Id + "' data-toggle='tooltip' title='Delete'><i class='fa fa-eye'></i></a> ";
                    link += "</div>"
                    return link;
                }
            }

        ],
        "initComplete": function (settings, json) {

        }

    });
}
$('[data-toggle="tooltip"]').tooltip();