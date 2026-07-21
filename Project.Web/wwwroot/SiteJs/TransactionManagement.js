$(document).ready(function () {
    BindTransactions(); 
});

/**
 * Bind Transactions List.
 */
function BindTransactions() {
    $('#Transactions_tbl').DataTable({
        "processing": true, // for show progress bar    
        "serverSide": true, // for process server side    
        ordering: false,
        searching: true,
        "lengthChange": false, 
        "pageLength": 10,
        language: {
            search: "",
            paginate: {
                next: '<i class="fas fa-arrow-right">', // or '→'
                previous: '<i class="fas fa-arrow-left">', // or '→'
            }
        },
        responsive: true,
        "order": [[0, "desc"]],
        "ajax": {
            url: window.appBase + "User/Transaction/GetTransactions",
            type: "POST",
            datatype: "json",
            complete: function (xhr, responseText) { 

            }
        },
        "columnDefs": [
        ],
        "columns": [
            {
                "render": function (data, type, row,item) {
                    return '<div class="relative mb-2 mt-2 flex grow items-center gap-2 px-6 sm:mb-0 sm:px-2 h-10">'+
                        '<div><span class="nui-paragraph nui-paragraph-xs nui-weight-normal nui-lead-tight text-muted-500 "><span class="bold_big">' + row.strCreatedOn + '</span></span>' +
                        '<h4 class="nui-heading nui-heading-sm nui-weight-medium nui-lead-tight text-muted-700 m-0"><span class="small_Light">' + row.transactionCode +'</span></h4>' +

                        '</div></div>';
                }
            },
            {
                "render": function (data, type, row, item) {
                    return '<div class="relative mb-2 mt-2 flex grow items-center gap-2 px-6 sm:mb-0 sm:px-2 h-10">' +
                        '<div>' +
                        '<span class="nui-paragraph nui-paragraph-xs nui-weight-normal nui-lead-tight text-muted-500 "><span >' + row.email + '</span></span>' +
                        '<h4 class="nui-heading nui-heading-sm nui-weight-medium nui-lead-tight text-muted-700 m-0"><span class="small_Light">' + row.userName + '</span></h4>' +

                        '</div></div>';
                }
            },
            {
                "render": function (data, type, row, item) {
                    return '<div class="bold_big">' + (row.transacionType)+" "+ row.currencyCode+ row.amount+'</div>';
                }
            },
            {
                "render": function (data, type, row, item) {
                    return '<div> <span class="tag tag-' + row.status +'">' + row.status +'</span></div>';
                }
            },
            {
                "data": "TransTypeString", "name": "TransTypeString",
                "render": function (data, type, row, item) {
                    return '<div> <span class="tag tag-' + row.transTypeString + '">' + row.transTypeString + '</span></div>';
                }
            },
            {
                "data": null, 
                "name": "Actions",
                "render": function (data, type, row) {
                    let refundAction = "";
                    //if (row.status == "Success")
                    //{
                    //    refundAction = '<a class="dropdown-item" onclick="refundTransaction(\'' + row.transactionCode + '\');" >Refund</a> ';
                    //}
                    return '<div class="dropdown flaxCenter">' +
                        '<button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fas fa-bars"></i></button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<a class="dropdown-item" onclick="ViewDetails(this);" data-id="' + row.transactionCode + '">Edit</a> ' +
                        refundAction +
                        '</div>' +
                        '</div>';
                },
                "orderable": false,
                "searchable": false  
            }
        ],
       "initComplete": function (settings, json) {
           $(".dataTables_filter input").attr("placeholder", 'Search'); 
           $(".dataTables_filter").append('<a class="btn btn-primary text-light" style="float:right" href="' + window.appBase + 'User/Transaction/Excel"><i class="fas fa-download"></i> <span>Donwload transactions</span></a>');
        }

    });
}
function ViewDetails(elem) {
    $("#DetailsModel").modal("show"); 
    $.post(window.appBase + "User/Transaction/GetTransactionsDetails", { transactionCode: $(elem).attr("data-id"), },
        function (data) { 
            $("#detailsPartial").html(data);
        }
    );
}


function refundTransaction(code) {

     
    Swal.fire({
        title: "Go to refund?", 
        showCancelButton: true,
        confirmButtonText: "Go",
    }).then((result) => { 
        if (result.isConfirmed) {
            window.location.href = window.appBase + "User/Transaction/refund?transaction=" + code;
        } 
    });


}


//$('#Transactions_tbl').on('click', '.btn-dark', function () {
//    debugger;
//    var transactionCode = $(this).data('id');
//    $.ajax({
//        type: "GET",
//        url: "/User/Transaction/GetTransactionsDetails", 
//        data: { Code: transactionCode },
//        success: function (response) {
//            console.log(response);
//            $('#TransactionModal .modal-body').html(response);
//            $('#TransactionModal').modal('show');
//        },
//        error: function (error) {
//            console.log("Error: ", error);
//        }
//    });
    
//});



