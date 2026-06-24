
//function printQR(tagid = "DivIdToPrint") {
//    var hashid = "#" + tagid;
//    var tagname = $(hashid).prop("tagName").toLowerCase();
//    var attributes = "";
//    var attrs = document.getElementById(tagid).attributes;
//    $.each(attrs, function (i, elem) {
//        attributes += " " + elem.name + " ='" + elem.value + "' ";
//    })
//    var divToPrint = $(hashid).html();
//    var head = "<html><head>" + $("head").html() + "</head>";
//    var allcontent = head + "<body  onload='window.print()' >" + divToPrint + "</body></html>";
//    var newWin = window.open('', 'Print-Window');
//    newWin.document.open();
//    newWin.document.write(allcontent);
//    newWin.document.close();
//    // setTimeout(function(){newWin.close();},10);
//}

function printQR(tagid = "DivIdToPrint") {

    var divToPrint = document.getElementById('DivIdToPrint');

    var newWin = window.open('', 'Print-Window');

    newWin.document.open(); 
    var ihtml = divToPrint.innerHTML.replace('height="250px"', 'height="450px"');
    ihtml = ihtml.replace('width="250px"', 'width="450px"');
    newWin.document.write('<html><body onload="window.print()">' + ihtml + '</body></html>');

    newWin.document.close();

    setTimeout(function () { newWin.close(); }, 10);

}