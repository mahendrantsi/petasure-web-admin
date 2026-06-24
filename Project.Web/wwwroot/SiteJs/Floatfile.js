// JavaScript Document
// version: beta
// created: 2005-08-30
// updated: 2005-08-31
// mredkj.com
function extractNumber(obj, decimalPlaces, allowNegative) {
    var temp = obj.value;

    // avoid changing things if already formatted correctly
    var reg0Str = '[0-9]*';


    if (decimalPlaces > 0) {
        reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
    } else if (decimalPlaces < 0) {
        reg0Str += '\\.?[0-9]*';
    }
    reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
    reg0Str = reg0Str + '$';
    var reg0 = new RegExp(reg0Str);
    if (reg0.test(temp)) return true;

    // first replace all non numbers
    var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
    var reg1 = new RegExp(reg1Str, 'g');
    temp = temp.replace(reg1, '');

    if (allowNegative) {
        // replace extra negative
        var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
        var reg2 = /-/g;
        temp = temp.replace(reg2, '');
        if (hasNegative) temp = '-' + temp;
    }

    if (decimalPlaces != 0) {
        var reg3 = /\./g;
        var reg3Array = reg3.exec(temp);
        if (reg3Array != null) {
            // keep only first occurrence of .
            //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
            var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
            reg3Right = reg3Right.replace(reg3, '');
            reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;


            temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;

        }

    }


    obj.value = temp;// addCommas(temp);
}

function DateFormate(obj, type) {


    var temp = obj.value;

    if (temp != "") {
        var newVal = parseInt(obj.value);
        if (type == "D") {
            if (newVal < 0 || newVal > 31) {
                obj.value = "";
            }

        }
        if (type == "M") {
            if (newVal < 0 || newVal > 12) {
                obj.value = "";
            }
        }
        if (type == "Y") {
            if (newVal < 1) {
                obj.value = "";
            }

        }


    }



}

function addCommas(obj) {
    var nStr = obj.value;
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    obj.value = x1 + x2;
}

function blockNonNumbers(obj, e, allowDecimal, allowNegative, allowhalfday) {
    //debugger;
    var key;
    var isCtrl = false;
    var keychar;
    var reg;

    if (window.event) {
        key = e.keyCode;
        isCtrl = window.event.ctrlKey
    }
    else if (e.which) {
        key = e.which;
        isCtrl = e.ctrlKey;
    }
    //add by bhupesh for allow halfday
    if (allowhalfday == true) {
        let objval = obj.value;
        if (obj.value.indexOf('.') != -1) {
            if (e.keyCode != 53) {
                return false;
            }
        }
    }

    if (isNaN(key)) return true;

    keychar = String.fromCharCode(key);

    // check for backspace or delete, or if Ctrl was pressed
    if (key == 8 || isCtrl) {
        return true;
    }

    reg = /\d/;
    var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
    var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

    return isFirstN || isFirstD || reg.test(keychar);
}

function SetYears(obj) {
    var nStr = obj.value;

    if (nStr == "") {
        nStr = 0;
    }

    if (parseInt(nStr) <= 0) {
        nStr = 1;
    }
    if (parseInt(nStr) > 35) {
        nStr = 35;
    }

    obj.value = nStr;
}