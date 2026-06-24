$(document).ready(function () {
    BindLogs();

});

/**
 * Bind Transactions List.
 */
function BindLogs() {
    $('#ErrorLogs_tbl').DataTable({
        "processing": true, // for show progress bar    
        "serverSide": true, // for process server side    
        "filter": true, // this is for disable filter (search box)    
        "orderMulti": false, // for disable multiple column at once 
        "pageLength": 10,
        ordering: false,
        info: false,
        "lengthChange": false,
        language: {
            search: "",
            paginate: {
                next: '<i class="fas fa-arrow-right">', // or '→'
                previous: '<i class="fas fa-arrow-left">', // or '→'
            },
            "zeroRecords": "<center>No record(s) found !</center>",
        },
        "initComplete": function (settings, json) {
            $(".dataTables_filter input").attr("placeholder", 'Search');

        },
        responsive: true,
        "order": [[0, "desc"]],
        "ajax": {
            url: "/Admin/TransactionManagement/GetErrorLog",
            type: "POST",
            datatype: "json",
            complete: function (xhr, responseText) {
            }
        },
        "columnDefs": [
        ],
        "columns": [
            {
                "render": function (data, type, row, item) {
                    return '<div class="relative mb-2 mt-2 flex grow items-center gap-2 px-6 sm:mb-0 sm:px-2 h-10">' +
                        '<div><span class="nui-paragraph nui-paragraph-xs nui-weight-normal nui-lead-tight text-muted-500 "><span class="bold_big">' + row.createdOn + '</span></span>' +
                        '<h4 class="nui-heading nui-heading-sm nui-weight-medium nui-lead-tight text-muted-700 m-0"><span class="small_Light">' + row.transactionCode + '</span></h4>' +

                        '</div></div>';
                }
            },
            {
                "render": function (data, type, row, item) {
                    return '<div class="relative mb-2 mt-2 flex grow items-center gap-2 px-6 sm:mb-0 sm:px-2 h-10">' +
                        '<div>' +
                        
                        '<h4 class="nui-heading nui-heading-sm nui-weight-medium nui-lead-tight text-muted-700 m-0"><span class="small_Light">' + row.payerName + '</span></h4>' +

                        '</div></div>';
                }
            },
            { "data": "exception", "name": "Exception", "autoWidth": true },
            { "data": "type", "name": "Type", "autoWidth": true },
            {
                "render": function (data, type, row, item) {
                    return ' <span class="tag tag-' + row.isSuccess + '">' + row.isSuccess + '</span>';
                }
            }
           
        ]

    });
}
