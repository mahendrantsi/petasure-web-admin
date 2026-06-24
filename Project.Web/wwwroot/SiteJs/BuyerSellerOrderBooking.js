$(document).ready(function () {
    BindOpenOrders();
    $("select[seller=seller_id]").trigger('change');
    $("select[buyer=buyer_id]").trigger('change');
});

function BindOpenOrders() {
    $.ajax({
        type: "GET",
        url: "/customer/exchange/GetOrderHistory",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { symbol: $('#hidcurrencysymbol').val() },
        success: function (response) {
            debugger;
            var template = $('#templateorderhistorytable').find('tbody');
            var templateHtml = $("#templateorderhistorytable").find("tbody").html();
            $('#orderhistorybook').find('tbody').empty();
            if (response.isSuccess) {
                $.each(response.content, (k, v) => {
                    template.find('tr td:eq(0)').find('span').text(v.createdTime)
                    template.find('tr td:eq(1)').find('span').text(v.orderType)
                    template.find('tr td:eq(2)').find('span').text(v.orderDirection)
                    template.find('tr td:eq(3)').find('span').text(v.orderPrice)
                    template.find('tr td:eq(4)').find('span').text(v.quantity)
                    template.find('tr td:eq(5)').find('span').text(parseFloat(v.quantity * v.orderPrice) )
                    template.find('tr td:eq(6)').find('span').text(v.execution)
                    template.find('tr td:eq(7)').find('span').text(v.orderStatus)
                    $('#orderhistorybook').find('tbody').append(template.html());
                    $('#templateorderhistorytable').find('tbody').html(templateHtml);
                });
            }
           
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }
    });
}
$(document).on('keyup', '#buy_volume', function () {
    if ($(this).val() != "") {
        let price = $('#buy_amount').val();
        let quantity = $(this).val();
        $('span[data-v-6d98d61c]').text(parseFloat(price * quantity));
    }
    else {
        $('span[data-v-6d98d61c]').text(0.00000000);
    }
});
$(document).on('keyup', '#sell_volume', function () {
    debugger;
    if ($(this).val() != "") {
        let price = $('#sell_amount').val();
        let quantity = $(this).val();
        $('span[data-v-6d98d62c]').text(parseFloat(price * quantity));
    }
    else {
        $('span[data-v-6d98d62c]').text(0.00000000);
    }
});

$(document).on('change', 'select[seller=seller_id]', function () {
    $('div[data-v-4621640sa]').hide();
    if (parseInt($(this).val()) == orderEnum.Market) {
        $("#sell_amount").attr('readonly', 'readonly');
    }
    else if (parseInt($(this).val()) == orderEnum.StopLimit || parseInt($(this).val()) == orderEnum.StopLoss) {
        $('div[data-v-4621640sa]').show();
        $("#sell_amount").removeAttr('readonly', 'readonly');
    }
    else {
        $("#sell_amount").removeAttr('readonly', 'readonly');
    }
});
$(document).on('change', 'select[buyer=buyer_id]', function () {
    debugger;
    $('div[data-v-4621640ba]').hide();
    if (parseInt($(this).val()) == orderEnum.Market) {
        $("#buy_amount").attr('readonly', 'readonly');
    }
    else if (parseInt($(this).val()) == orderEnum.StopLimit || parseInt($(this).val()) == orderEnum.StopLoss) {
        $('div[data-v-4621640ba]').show();
        $("#sell_amount").removeAttr('readonly', 'readonly');
    }
    else {
        $("#buy_amount").removeAttr('readonly', 'readonly');
    }
});
function asyncTradingPrice(symbol, currentprice) {
        debugger;
        var activeTab = $("ul.order-tab li a.active").text();
        if (activeTab == "Buy") {
            var orderobj = $('select[buyer=buyer_id]');
            let volumnobj = $('#buy_volume');
            let orderpriceobj = $('#buy_amount');
            let hidorderpriceobj = $('#buy_amount');
            let totalobj = $('span[data-v-6d98d61c]');
            BuyerSellerOrder(symbol, currentprice, orderobj, volumnobj, orderpriceobj, hidorderpriceobj, totalobj);
        }
        else if (activeTab == "Sell") {
            var orderobj = $('select[seller=seller_id]');
            let volumnobj = $('#sell_volume');
            let orderpriceobj = $('#sell_amount');
            let hidorderpriceobj = $('#sell_amount');
            let totalobj = $('span[data-v-6d98d62c]');
            BuyerSellerOrder(symbol, currentprice, orderobj, volumnobj, orderpriceobj, hidorderpriceobj, totalobj);
        }
    }
        function BuyerSellerOrder(symbol, currentprice, orderobj, volumnobj, orderpriceobj, hidorderpriceobj, totalobj) {
            if (parseInt(isDefault(orderobj.val())) != 0) {
                if (parseInt(isDefault(orderobj.val())) == orderEnum.Market) {

                    if (parseFloat(volumnobj.val() == "" ? 0 : volumnobj.val()) > 0) {
        let priceAmt = parseFloat(currentprice) * parseFloat(volumnobj.val());
                        orderpriceobj.val(currentprice)
                        hidorderpriceobj.val(currentprice);
                        totalobj.text(currentprice);
                    }
                    else {
        orderpriceobj.val(currentprice);
                        hidorderpriceobj.val(currentprice);
                        totalobj.text(0.00000000);
                    }
                }
            }
            else {
        orderpriceobj.val(currentprice);
                hidorderpriceobj.val(currentprice);
                totalobj.text(0.00000000);
            }
        }
        function isEmpty(val) {
            return (val === undefined || val == null || val.length <= 0) ? true : false;
        }
        function isDefault(val) {
            return (val === undefined || val == null || val.length <= 0 || isNaN(val) || val == "") ? 0 : val;
        }
        var orderEnum = {
        Market: 1,
            Limit: 2,
            StopLimit: 3,
            StopLoss: 4
        }