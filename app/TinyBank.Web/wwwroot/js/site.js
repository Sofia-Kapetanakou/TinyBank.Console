// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$('.js-update-customer').on('click',
    (event) => {
        debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();

        console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName
        });

        // ajax call
        let result = $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            // success
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });



$('.js-card-pay').on('click',
(event) => {

    $('#cardNumberInput').prop('disabled', true);
    let cardNumber = $('#cardNumberInput').val();
    let expirationMonth = $('#expirationMonthInput').val();
    let expirationYear = $('#expirationYearInput').val();
    let amount = $('#amountInput').val();

    let data = JSON.stringify({
        cardNumber: cardNumber,
        expirationMonth: expirationMonth,
        expirationYear: expirationYear,
        amount: amount
    });

    // ajax call
    let result = $.ajax({
        url: `/card/checkout`,
        method: 'POST',
        contentType: 'application/json',
        data: data
    }).done(response => {       
        // success
        debugger;       
        if (response.success) {
            $('.js-show-success').show();
            $('.js-show-fail').hide();            
        }
        else {
            $('.js-error-text').text(response.responseText);
            $('.js-show-fail').show();
            $('.js-show-success').hide(); 
        }
    }).always(
        $('#cardNumberInput').prop('disabled', false)
    );
});