
selectedCopies = []

$(document).ready(function () {
    $('.js-search').on('click', function (e) {
        e.preventDefault();
         var serial = $('#Value').val();
        if (selectedCopies.find(c => c.serial == serial)) {
            showErrorMessage("you cannot add the same copy")
            return;
        }

        if (selectedCopies.length >= maxAllowedCopies) {
            showErrorMessage(`you cannot add more than ${maxAllowedCopies} books`)
            return;
        }
        $('#SearchForm').submit();

    })


    $('body').delegate('.js-remove', 'click', function () {

        $(this).parents('.js-copy-container').remove();

        if (selectedCopies.length == 0)
        { console.log("dddd")
            $('#CopiesForm').find(':submit').addClass('d-none');
        }
          
        prepareInput()

    })

})
function onAddCopySuccess(copy) {
    $('#Value').val('');

    var bookId = $(copy).find('.js-copy').data('book-id');

    if (selectedCopies.find(c => c.bookId == bookId)) {
        showErrorMessage("you cannot add more than one copy for the same book")
        return;
    }

    $('#CopiesForm').prepend(copy);
    $('#CopiesForm').find(':submit').removeClass('d-none');
    prepareInput()
}

function prepareInput() {
    var copies = $('.js-copy');
    selectedCopies = []
    $.each(copies, function (i, input) {
        var $input = $(input);
        selectedCopies.push({ serial: $input.val(), bookId: $input.data('book-id') });
        $input.attr('name', `SelectedCopies[${i}]`).attr('id', `SelectedCopies_${i}_`)
    })
}