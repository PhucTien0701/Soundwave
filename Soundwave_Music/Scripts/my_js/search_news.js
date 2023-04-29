$(document).ready(function () {
    $("#News_title").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/NewsHome/SuggestNewsSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.News_title, value: item.News_title, News_id: item.News_id, Image: item.Image };
                    }))
                    if (data.length == 0) {
                        $('.spinner').show();
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'center',
                            showConfirmButton: false,
                            timer: 1500,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })
                        Toast.fire({
                            icon: 'error',
                            title: 'This news is not available'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.Image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchnews">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/NewsHome/NewsDetail/' + ui.item.News_id;
            return false;
        }
    })
})