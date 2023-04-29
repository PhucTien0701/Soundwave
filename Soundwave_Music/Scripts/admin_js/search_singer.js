$(document).ready(function () {
    $("#Singer_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Singer/SuggestSingerSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Singer_name, value: item.Singer_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No singer founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Singer_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Singer_name").val(ui.item.label);
            return false;
        }
    })

})