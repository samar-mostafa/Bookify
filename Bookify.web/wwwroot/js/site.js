
var updatedRow,
 table,
 datatable,
exportedCloumns = [];

//alerts
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

//model
function onModelBegin() {
   
    $('.js-loading').attr("disabled", "disabled").attr('data-kt-indicator','on');
}
function onModelSuccess(row) {

    showSuccessMessage();
    window.$('#Model').modal('hide');
    if (updatedRow !== undefined) {
        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }  
    var newRow = $(row);
    datatable.row.add(newRow).draw();

    KTMenu.init();
    KTMenu.initHandlers();
}
function onModelComplete() {
    $('body:submit').removeAttr('disabled');
}

//datatables
var KTDatatables = function () {
    //exclude action cloum
    var headres = $('th');
    $.each(headres, function (i) {       
        var col = $(this);
        if (!col.hasClass('js-no-export'))             
            exportedCloumns.push(i);  
    })
  
    // Private functions
    var initDatatable = function () {

        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $(".js-datatables").data("documentTitle");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCloumns
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCloumns
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCloumns
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCloumns
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js_datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
$(document).ready(function () {
    //datatable
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });
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

    //handel toggle status
    $('body').delegate('.js-toggle-status', 'click', function () {
        var btn = $(this);
        var btnStatusId = btn.data('id');
        bootbox.confirm({
            message: 'are you sure to change the category status?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result)
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (updatedOn) {
                            var row = btn.parents('tr');
                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                            row.find('.js-updated-on').html(updatedOn)
                            showSuccessMessage("status changed successfully");
                            row.addClass("animate__animated animate__headShake");
                        },
                        error: function () {
                            showErrorMessage()
                        }
                    })
            }
        });


    })

})
