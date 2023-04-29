$(document).ready(function () {
    $("#Genre_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Genre/SuggestGenreSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Genre_name, value: item.Genre_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No genre founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Genre_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Genre_name").val(ui.item.label);
            return false;
        }
    })

})