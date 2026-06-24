/*
 * Click Event by feerange class.
 * Add fee range row.
 * */
$(document).on('click', '.feerange', function () {
    debugger;
    $('.clsfeerange').removeClass('hidden');
    var template = $("#templatetable").find("tbody");
    var templateHtml = $("#templatetable").find("tbody").html();
    var totalRows = $('#feerangetbl').find('tbody tr').length;
    template.find('tr').attr('id', "row_" + totalRows);
    template.find('.txtfromamt').attr('name', "feeRangeMasterViewModel[" + totalRows + "].FromAmount");
    template.find('.txttoamt').attr('name', "feeRangeMasterViewModel[" + totalRows + "].ToAmount");
    template.find('.txtfee').attr('name', "feeRangeMasterViewModel[" + totalRows + "].Fee");
    $('#feerangetbl').find('tbody').append(template.html());
    $('#templatetable').find('tbody').html(templateHtml);

});

/*
 * Button click event calling.
 * */

$(document).on('click', 'input[type=button]', function () {
    event.preventDefault();
    debugger;
    var row = $('#feerangetbl').find('tbody tr');
    let feeIsValidate = true;
    var isValid = true;
    var rangeArray = [];
    row.each(function (i) {
        let itemArr = {
            fromAmt: $(this).find(".txtfromamt").val(),
            toamt: $(this).find(".txttoamt").val(),
            fee : $(this).find(".txtfee").val()
        }
        rangeArray.push(itemArr);
    })
    for (var i = 0; i < rangeArray.length; i++) {
        if (isEmpty(rangeArray[i].fromAmt) || isEmpty(rangeArray[i].toamt) || isEmpty(rangeArray[i].fee)) {
            bootbox.alert({
                message: "Please fill Range Amount.",
                size: 'small',
            }).find('.modal-dialog').addClass("modal-dialog-centered");
            return false;
        }
        for (var j = i + 1; j < rangeArray.length; j++) {
            if ((parseFloat(rangeArray[j].fromAmt) >= parseFloat(rangeArray[i].fromAmt)) && (parseFloat(rangeArray[j].fromAmt) <= parseFloat(rangeArray[i].toamt))) {
                debugger;
                isValid = false;
            }
            else if ((parseFloat(rangeArray[j].toamt) >= parseFloat(rangeArray[i].fromAmt)) && (parseFloat(rangeArray[j].toamt) <= parseFloat(rangeArray[i].toamt))) {
                isValid = false;
            }
        }
    }
    if (isValid == true) {

            $("#feeForm").submit();
        }
    else
        bootbox.alert({
            message: "Amount is already in the Range.Please select other Amount.",
            size: 'small',
        }).find('.modal-dialog').addClass("modal-dialog-centered");
    return false;


});

/**
 * DeleteRecord Entry.
 * @param {any} obj
 */

function DeleteRecord(obj) {    
    $(obj).closest('tr').remove();
    var row = $('#feerangetbl').find('tbody tr').length;
    if (row == 0) {
        $('.clsfeerange').addClass('hidden');
    }
}

/**
 * Check empty string validator.
 * @param {any} val
 */
function isEmpty(val) {
    return (val === undefined || val == null || val.length <= 0) ? true : false;
}