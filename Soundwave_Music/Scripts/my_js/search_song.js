$(document).ready(function () {
    $("#Song_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/UserSong/SuggestSongSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Song_name, value: item.Song_name, Song_id: item.Song_id, Image: item.Image };
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
                            title: 'This song is not available'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.Image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchsong">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/UserSong/SongDetail/' + ui.item.Song_id;
            return false;
        }
    })
})