$(document).ready(function () {


    $('.js-renew').on('click', function () {

        var subscriptionKey = $(this).data('key');
        console.log(subscriptionKey)
        bootbox.confirm({
            message: 'are you sure to renew this subscription?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result)
                    $.post({
                        url: `/Subscripers/RenewSubscription?sKey=${subscriptionKey}`,
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (row) {
                            $('#SubscriptionsTable').find('tbody').append(row);
                            var ActiveStatusIcon = $('#ActiveStatusIcon');
                            ActiveStatusIcon.removeClass('d-none');
                            ActiveStatusIcon.siblings('svg').remove()
                            ActiveStatusIcon.parents('.card').removeClass('bg-warning').addClass('bg-success');
                            $('#CardStatus').text('Active Subscriber');
                            $('#StatusBadge').removeClass('badge-light-warning').addClass('badge-light-success').text('Active Subscriber')
                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage()
                        }
                    })
            }
        });

    })
})