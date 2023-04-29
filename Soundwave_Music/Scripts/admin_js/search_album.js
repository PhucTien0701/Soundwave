$(document).ready(function () {
    $("#Album_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Album/SuggestAlbumSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Album_name, value: item.Album_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No album founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Album_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Album_name").val(ui.item.label);
            return false;
        }
    })

})