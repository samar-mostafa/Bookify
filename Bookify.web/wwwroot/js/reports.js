
$(document).ready(function () {
    $('.js_daterangepicker').daterangepicker({
        autoApply: true,
        showDropdowns: true,
        minYear: 2020,
        maxYear: new Date().getFullYear(),
        maxDate: new Date(),
        autoUpdateInput: true

    });
    $('.js_daterangepicker').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
    });


    $('.page-link').on('click', function () {
        var btn = $(this);
        var pageNumber = btn.data('page-number');
        if (btn.parent().hasClass('active')) return;

        $('#PageNumber').val(pageNumber);
        $('#Filters').submit();

    });
})