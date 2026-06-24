
    // Initialize Select2
$(document).ready(function () {
    $('.customDropdown').each(function () { 
        $(this).select2({
            templateResult: formatOption,
            templateSelection: formatState
        });
    });
    //$("#btnSubmit").click(function (e) {
    //    e.preventDefault();
    //    alert();
    //    if (!$("#menuForm").valid()) {
    //        alert()
    //    }
    //    return false;

       
    //});
});

    // Function to format the options
    function formatOption(option) {
        if (!option.id) {
            return option.text;
        }

    var $option = $(
    '<span><img src="' + $(option.element).data('image') + '" class="img-flag" height="10px" width="10px" /> ' + option.text + '</span>'
    );

    return $option;
    }



    function formatState(state) {
        if (!state.id) {
            return state.text;
        }

    var baseUrl = "/images/country-flags";
    var $state = $(
    '<span><img class="img-flag" /> <span></span></span>'
    );

    // Use .text() instead of HTML string concatenation to avoid script injection issues
    $state.find("span").text(state.text);
    $state.find("img").attr("src", $(state.element).data('image'));

    return $state;
    };
