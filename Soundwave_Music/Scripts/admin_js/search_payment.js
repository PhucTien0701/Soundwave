$(document).ready(function () {
    $("#Payment_method").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Payment/SuggestPaymentSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Payment_method, value: item.Payment_method };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No payment founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Payment_method").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Payment_method").val(ui.item.label);
            return false;
        }
    })

})