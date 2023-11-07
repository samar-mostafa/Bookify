
var updatedRow;
function showSuccessMessage(message = 'Saved successfully') {
    Swal.fire({
        text: message,
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function showErrorMessage(message = 'something went wrong') {
    Swal.fire({
        text: message,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}
function onModelSuccess(item) {

    showSuccessMessage();
    window.$('#Model').modal('hide');
    if (updatedRow === undefined) {
        $('tbody').append(item);
    }
    else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }
    KTMenu.init();
    KTMenu.initHandlers();
}

$(document).ready(function () {

    //show edit and add success message
    var message = $('#message').text();
    if (message !== '') {
        showSuccessMessage(message);
    }

    //dealing with model
    $('body').delegate('.js-model-render','click', function () {
        var btn = $(this);
        var model = $('#Model');
        model.find('#ModelLabel').text(`${btn.data('title')}`);
        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }
        $.get({
            url: btn.data('url'),
            success: function (form) {
                model.find('#modelBody').html(form);
               /* if ($.validator.unobtrusive != undefined) {*/
                    $.validator.unobtrusive.parse(model);
                /*}*/
            },
            error: function () {
                showErrorMessage();
            }
        })
        window.$('#Model').modal('show');
       // mod.model('show');
       
    })

})
