$(document).ready(function () {
    $("#Video_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Video/SuggestVideoSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Video_name, value: item.Video_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No video founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Video_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Video_name").val(ui.item.label);
            return false;
        }
    })

})