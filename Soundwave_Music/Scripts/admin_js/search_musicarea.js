$(document).ready(function () {
    $("#Area_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/MusicAreas/SuggestAreasSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Area_name, value: item.Area_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No area founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Area_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Area_name").val(ui.item.label);
            return false;
        }
    })

})