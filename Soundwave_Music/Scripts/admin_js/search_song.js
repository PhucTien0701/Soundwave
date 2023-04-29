$(document).ready(function () {
    $("#Song_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Song/SuggestSongSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Song_name, value: item.Song_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No song founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Song_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Song_name").val(ui.item.label);
            return false;
        }
    })

})