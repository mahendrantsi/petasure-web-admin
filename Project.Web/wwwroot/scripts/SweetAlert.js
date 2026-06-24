

    function SwalSave(elem,msg,url) {

        // alert($(elem).closest("form").length)
        // var validator = $(elem).closest("form").validate();

        // if (validator) {

        Swal.fire({
            title: 'Are you sure?',
            icon: 'warning',
            html: msg,
            showCancelButton: true,
            confirmButtonColor: '#294fc0',
            cancelButtonColor: '#ff5e5e',
            confirmButtonText: 'Confirm'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = url;
            }
        });
    //}
    event.preventDefault();
        }

    function SwalOk(){
        Swal.fire({
            position: 'Center',
            icon: 'success',
            title: 'Your work has been saved',
            showConfirmButton: false,
            timer: 1500
        })

    }