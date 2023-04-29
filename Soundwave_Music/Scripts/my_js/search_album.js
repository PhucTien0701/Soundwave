$(document).ready(function () {
    $("#Album_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/UserAlbum/SuggestAlbumSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Album_name, value: item.Album_name, Album_id: item.Album_id, Image: item.Image };
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
                            title: 'This album is not available'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.Image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchalbum">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/UserAlbum/AlbumDetail/' + ui.item.Album_id;
            return false;
        }
    })
})