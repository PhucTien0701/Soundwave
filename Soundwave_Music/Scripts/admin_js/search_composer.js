$(document).ready(function () {
    $("#Composer_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Composer/SuggestComposerSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Composer_name, value: item.Composer_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No composer founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Composer_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Composer_name").val(ui.item.label);
            return false;
        }
    })

})