$(document).ready(function () {
    $("#Singer_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/UserSinger/SearchSingerResult",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Singer_name, value: item.Singer_name, image: item.Image};
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
                            title: 'This singer is not available'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchsinger">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/singer/' + ui.item.Singer_name;
            return false;
        }
    })

})