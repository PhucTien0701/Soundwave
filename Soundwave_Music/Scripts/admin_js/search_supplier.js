$(document).ready(function () {
    $("#Supplier_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Supplier/SuggestSupplierSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Supplier_name, value: item.Supplier_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "No supplier founded.",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Supplier_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Supplier_name").val(ui.item.label);
            return false;
        }
    })

})