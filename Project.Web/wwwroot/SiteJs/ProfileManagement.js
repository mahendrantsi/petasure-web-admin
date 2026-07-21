var userInstitutionId = 0;
var Step = 1;
$(document).ready(function () {
    $('.nav-tabs > li a[title]').tooltip();


    //Wizard
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

        var target = $(e.target);

        if (target.parent().hasClass('disabled')) {
            return false;
        }
    });

    // $(".btn-dark.next-step").click(function (e) {
    //     var active = $('.wizard .nav-tabs li.active');
    //     active.next().removeClass('disabled');
    //     nextTab(active);
    // });
    // $(".prev-step").click(function (e) {
    //     var active = $('.wizard .nav-tabs li.active');
    //     prevTab(active);

    // });
});



function stepUpProgress() {

    Step++;
    var currentStep = Step;
    var currentPos = currentStep * 20;
    $(".stepName").html($('.wizard .nav-tabs li.active').find("a").attr("stepName"));
    $(".stepCounter").html(currentStep);
    $(".progress").css("width", (currentPos) + "%");
}

function nextTab(elem) {
    var active = $('.wizard .nav-tabs li.active');

    /*var currentStep = parseInt($(active).find("a").attr("currentStep")) + 1*/;
    $(active).next().find('a[data-toggle="tab"]')[0].click();
    stepUpProgress();
    active.next().removeClass('disabled');

}
function prevTab(elem) {
    var active = $('.wizard .nav-tabs li.active');
    //var currentStep = parseInt($(active).find("a").attr("currentStep")) - 1;
    Step--;
    var currentStep = Step;

    var currentPos = currentStep * 20;
    $(".stepName").html($(active).prev('li').find("a").attr("stepName"));
    $(".stepCounter").html(currentStep);

    if ($("#chkUser").is(':checked')) {

        if ($(active).find('a').attr("href") == "#step5") {
            $('.nav-tabs a[href="#step2"]')[0].click();
            return;
        }
    }

    $(".progress").css("width", (currentPos) + "%")
    $(active).prev().find('a[data-toggle="tab"]')[0].click();
}

$('.nav-tabs').on('click', 'li', function () {
    $('.nav-tabs li.active').removeClass('active');
    $(this).addClass('active');
});


$(document).ready(function () {
    const $isMerchantCheckbox = $('#isMerchantCheckbox');
    const $btnSubmit = $('.Finish');
    const $btnNext = $('.Merchant');
    const $merchantProfileTab = $('#merchantProfileTab');
    const $kycSection = $('#kycSection');

    $isMerchantCheckbox.change(function () {
        debugger;
        if ($(this).prop('checked')) {
            $merchantProfileTab.removeClass('hidden-tab');
            $btnSubmit.addClass('hidden-tab');
            $kycSection.addClass('hidden-tab');
            $btnNext.removeClass('hidden-tab');
            $('#KYCDocuments').val('');
        } else {
            $merchantProfileTab.addClass('hidden-tab');
            $('#step3').find('input, textarea, select').val('');
            $btnSubmit.removeClass('hidden-tab');
            $kycSection.removeClass('hidden-tab');
            $btnNext.addClass('hidden-tab');

            if ($('#step3').hasClass('active')) {
                $('.nav-tabs a[href="#tab1"]').tab('show');
            }
        }
    });



    $("#AddressCountryID,#StrDateOfBirth").change(function () {
        if (!$(this).valid()) {
            $(this).closest("div.form-group").find(".select2>.selection>span").addClass("input-validation-error2 input-validation-error")
        }
        else {
            $(this).closest("div.form-group").find(".select2>.selection>span").removeClass("input-validation-error2 input-validation-error")
        }

    });

    $('.CheckVaildation').click(function (e) {
        e.preventDefault();

        var isValid = true;
        var elements = $('.CheckVaildation').closest('.tab-pane.active')
        $('.tab-pane.active input,.tab-pane.active textarea,.tab-pane.active select').each(function () {
            if (!$(this).valid()) {
                isValid = false;
                if ($(this).hasClass("customDropdown")) {
                    $(this).closest("div.form-group").find(".select2>.selection>span").addClass("input-validation-error2")
                }
            }
            else { 
                if ($(this).hasClass("customDropdown")) {
                    $(this).closest("div.form-group").find(".select2>.selection>span").removeClass("input-validation-error2")
                }
            }
        });

        if (isValid) {
            // var active = $('.wizard .nav-tabs li.active');
            // active.next().removeClass('disabled');
            // $(active).next().find('a[data-toggle="tab"]')[0].click();
            nextTab(e);
            BindDetails();
        }
    });

});

function CheckBankValidation(elem) {


    var active = $('.wizard .nav-tabs li.active');
    active.next().removeClass('disabled');
    $(active).next().find('a[data-toggle="tab"]')[0].click();
}
$(".closeBank").click(function () {
    var selectedIndex = $("#bankModal").attr('data-selected-index');
    $("input[name='lstBank[" + selectedIndex + "].Selected']").prop('checked', false)
    $("#AccountNo").val('');
    $("#SortCode").val('');
    $('#IsPrimary').prop('checked', false);
    $("#bankModal").modal("hide");
})
$(".bank-checkbox").change(function () {
    if ($(this).is(":checked")) {
        $("#AccountNo").val('');
        $("#SortCode").val('');
        $('#IsPrimary').prop('checked', false);
        $("#bankModal").attr('data-selected-index', $(this).data('institution-index'));
        $("#bankModal").modal({ backdrop: 'static', keyboard: false }, "show");
    }
    else { 
        $(this).closest('label').removeClass("bank-checked");
    }
});

$(".saveBankDetails").click(function (e) {
    //check validation
    var isValid = true;
    $('.accountdetails input').each(function () {
        if (!$(this).valid()) {
            isValid = false; 
        }
    });
    if (!isValid) {
        return false;
    }
    else {
        var accountNo = $("#AccountNo").val();
        var sortCode = $("#SortCode").val();
        var IsPrimary = $("#IsPrimary").is(":checked");
        $(".primary").each(function () {
            $(this).prop('checked', false);
        });

        // Retrieve the institution index from the modal
        var selectedIndex = $("#bankModal").attr('data-selected-index');

        if (IsPrimary == true) {
            var exists = true;
            var i = 0;
            while (exists) {

                if ($("input[name='lstBank[" + i + "].IsPrimary']").length > 0) {
                    $("input[name='lstBank[" + i + "].IsPrimary']").prop('checked', false);
                    i++;
                }
                else {
                    exists = false;
                }
            }
        }
        
        // Update the hidden input fields using the institution index

        $("input[name='lstBank[" + selectedIndex + "].AccountNo']").val(accountNo);
        $("input[name='lstBank[" + selectedIndex + "].SortCode']").val(sortCode);
        $("input[name='lstBank[" + selectedIndex + "].IsPrimary']").prop('checked', IsPrimary);
        $("input[name='lstBank[" + selectedIndex + "].IsPrimary']").val(IsPrimary);
        console.log($("input[name='lstBank[" + selectedIndex + "].IsPrimary']").is(':checked')==true);
        $("input[name='lstBank[" + selectedIndex + "].Flag']").val(accountNo);
        $("input[name='lstBank[" + selectedIndex + "].AccountNo']").closest('label').addClass("bank-checked");

        //Clear the values
        $("#AccountNo").val('');
        $("#SortCode").val('');
        $('#IsPrimary').prop('checked', false);
        $("#bankModal").modal("hide"); // close the modal


    }
    e.preventDefault()
});


$(".docPreview").click(function () {
    $('.loader').removeClass('d-none');
    var docId = $(this).data('id');
    $.get(window.appBase + "Admin/UserManagement/GetDocumentById?Id=" + docId, function (res) {
        if (res) {
            if (res.response.isSuccess) {
                $("#DocumentPreviewModal").modal("show");
                var fileExt = res.fileName.substr(res.fileName.lastIndexOf('.') + 1);
                if (fileExt == "pdf") {
                    $('#ImgViewer').attr("src", "");
                    $('#ImgViewer').addClass('d-none');
                    $('#DocViewer').removeClass('d-none');
                    $('#DocViewer').attr("src", res.response.data);
                }
                else {
                    $('#DocViewer').attr("src", "");
                    $('#DocViewer').addClass('d-none');
                    $('#ImgViewer').removeClass('d-none');
                    $('#ImgViewer').attr("src", res.response.data);
                }
            }
            else
                Swal.fire({ title: "Error!", text: res.message, icon: "error" });
            $('.loader').addClass('d-none');
        }
    });
});


$(".AddBank").click(function () {

    if (userInstitutionId != undefined)
        userInstitutionId = 0;
    $("#BankId").val("0").change();
    $("#accountNo,#sortCode").val("");
    $("#BankId").removeAttr("disabled");
    $("#AddBankModal").modal("show");
    $('#IsPrimary').prop('checked', false);
});

//Saving of Update Profile , bank Details
//$(".AddBankAccountDetails").click(function () {
//    debugger;
//    //check validation
//    var isValid = true;
//    $('.AddBankAccountDetails input').each(function () {
//        if (!$(this).valid()) {
//            isValid = false;
//            return false;
//        }
//    });
//    if (isValid) {
//        console.log("valid")

//        var data = {
//            AccountNo: $("#accountNo").val(),
//            SortCode: $("#sortCode").val(),
//            Value: $("#BankId").val()
//        };

//        $.ajax({
//            type: "POST",
//            url: "/User/Account/SaveBankDetails",
//            data: data,
//            success: function (response) {
//                console.log(response);


//                /*
//                $.get("/User/Account/GetBankDetails", function (res) {
//                    if (res) {
//                        $('#BankDetails').html(res);
//                    }
//                });
//                */
//            },
//            error: function (error) {
//                console.log("Error: ", error);
//            }
//        });


//        //Clear the values
//        $("#accountNo").val('');
//        $("#sortCode").val('');
//        $("#BankId").val('');
//        $("#bankModal").modal("hide"); // close the modal
//    }
//});



/////////////////Profile Edit and delete//////////

$(document).on('click', '.btnBankDelete', function (e) {

    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure you want to delete bank account!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }).then((result) => {
        if (result.isConfirmed) {
            e.preventDefault();
            $('.loader').show();
            var id = $(this).data('bankid');
            $.ajax({
                url: window.appBase + 'User/Account/DeleteBankDetails',
                type: 'GET',
                data: { id: id },
                success: function (response) {
                    if (response.isSuccess) {
                        $.get(window.appBase + "User/Account/GetBankDetails", function (res) {
                            if (res) {
                                $('#BankDetails').html(res);
                                $('#AddBankModal').modal('hide');
                                $('.loader').hide();
                            }
                        });
                    } else {
                        alert('Error: ' + response.message);
                        $('.loader').hide();
                    }
                },
                error: function () {
                    alert('An error occurred while deleting.');
                    $('.loader').hide();
                }
            });
        }
    });
});

$("#IsKyc").change(function () {
    $('#FailReason').addClass("d-none");
    //if ($('option:selected', this).val() == 4) //for fail
    //{
    $('#FailReason').removeClass("d-none");
    //}
});

$("#KYBStatus").change(function () {
    $('#FailReason').addClass("d-none");
    //if ($('option:selected', this).val() == 4) //for fail
    //{
    $('#FailReason').removeClass("d-none");
    //}
});


$(document).on('click', '.btnBankSetPrimary', function (e) {

    debugger;
    e.preventDefault();
    $('.loader').show();
    var id = $(this).data('bankid');
    $.ajax({
        url: window.appBase + 'Admin/Account/SetPrimaryAccount?id=' + id,
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.isSuccess) {
                $.get(window.appBase + "Admin/Account/GetBankDetails?userID=" + $("#Id").val(), function (res) {
                    if (res) {
                        $('#BankDetails').html(res);
                        $('#AddBankModal').modal('hide');
                        $('.loader').hide();
                        Swal.fire({
                            title: "Success!",
                            text: "Primary account has been switch!",
                            icon: "success"
                        });
                    }
                });
            } else {
                alert('Error: ' + response.message);
                $('.loader').hide();
            }
        },
        error: function () {
            $('.loader').hide();
        }
    });
});




$(document).on('click', '.btnUserBankSetPrimary', function (e) {

    debugger;
    e.preventDefault();
    $('.loader').show();
    var id = $(this).data('bankid');
    $.ajax({
        url: window.appBase + 'User/Account/SetPrimaryAccount?id=' + id,
        type: 'POST',
        data: { id: id },
        success: function (response) {
            if (response.isSuccess) {
                $.get(window.appBase + "User/Account/GetBankDetails", function (res) {
                    if (res) {
                        $('#BankDetails').html(res);
                        $('#AddBankModal').modal('hide');
                        $('.loader').hide();
                        Swal.fire({
                            title: "Success!",
                            text: "Primary account has been switch!",
                            icon: "success"
                        });
                    }
                });
            } else {
                alert('Error: ' + response.message);
                $('.loader').hide();
            }
        },
        error: function () {
            $('.loader').hide();
        }
    });
});






$(document).on('click', '.btnBankDeleteAdmin', function (e) {
    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure you want to delete bank account!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
    }).then((result) => {
        if (result.isConfirmed) {
            e.preventDefault();
            $('.loader').show();
            var id = $(this).data('bankid');
            $.ajax({
                url: window.appBase + 'Admin/UserManagement/DeleteBankDetails',
                type: 'GET',
                data: { id: id, userID: $("#Id").val() },
                success: function (response) {
                    if (response.isSuccess) {
                        $.get(window.appBase + "Admin/Account/GetBankDetails?userID=" + $("#Id").val(), function (res) {
                            if (res) {
                                $('#BankDetails').html(res);
                                $('#AddBankModal').modal('hide');
                                $('.loader').hide();
                            }
                        });
                    } else {
                        alert('Error: ' + response.message);
                        $('.loader').hide();
                    }
                },
                error: function () {
                    alert('An error occurred while deleting.');
                    $('.loader').hide();
                }
            });
        }
    });
});



$(document).on('click', '.btnBankEditAdmin', function (e) {
    e.preventDefault();
    userInstitutionId = $(this).data('bankid')

    $.ajax({
        url: window.appBase + 'Admin/UserManagement/GetBankDetailsById',
        type: 'GET',
        data: { userInstitutionId: userInstitutionId },
        success: function (response) {
            if (response.isSuccess) {
                // set the fields
                $('#AddBankModal').modal('show');
                $('#BankId').attr('disabled', true)
                populateFormNew(response.data);



            } else {
                alert('Error: ' + response.message);

            }
        },
        error: function () {
            alert('An error occurred while deleting.');
            $('.loader').hide();
        }
    });
});


$(document).on('click', '.btnBankEdit', function (e) {

    debugger;
    e.preventDefault();

    userInstitutionId = $(this).data('bankid')

    $.ajax({
        url: window.appBase + 'User/Account/GetBankDetailsById',
        type: 'GET',
        data: { userInstitutionId: userInstitutionId },
        success: function (response) {
            if (response.isSuccess) {
                // set the fields
                $('#AddBankModal').modal('show');
                $('#BankId').attr('disabled', true)
                populateFormNew(response.data);


            } else {
                alert('Error: ' + response.message);

            }
        },
        error: function () {
            alert('An error occurred while deleting.');
            $('.loader').hide();
        }
    });
});

function populateFormNew(data) {
    $("#BankId").val(data.bankId);
    $("#accountNo").val(data.accountNo);
    $("#sortCode").val(data.shortCode);
    $("#IsPrimary").prop('checked', data.isPrimary);
    // Setting the entire data object as 'bank' data attribute for #BankId element
    $("#BankId").data('bank', data);  // This sets the JSON object in memory but not visually in the DOM

}

function populateForm(data) {
    $("#BankId").val(data.yapilyBankId);
    $("#accountNo").val(data.accountNo);
    $("#sortCode").val(data.shortCode);

    // Setting the entire data object as 'bank' data attribute for #BankId element
    $("#BankId").data('bank', data);  // This sets the JSON object in memory but not visually in the DOM

}