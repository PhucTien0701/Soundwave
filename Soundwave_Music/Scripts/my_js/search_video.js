$(document).ready(function () {
    $("#Video_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/UserVideo/SearchVideoResult",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Video_name, value: item.Video_name, image: item.Image};
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
                            title: 'This video is not available'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchvideo">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/video/' + ui.item.Video_name;
            return false;
        }
    })

})