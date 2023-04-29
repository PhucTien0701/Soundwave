$(document).ready(function () {
    $('.form_comment_post').summernote({
        height: 100,
        placeholder: 'Để lại bình luận của bạn tại đây...',
    });
    //set tên thôi
    $('.note-icon-picture').click(function () {
        $('.note-image-btn').val('Chèn');
        $('.modal-title').text('Chèn ảnh');
        $('.note-group-select-from-files .note-form-label').text('Chọn từ File');
        $('.note-group-image-url .note-form-label').text('URL ảnh');
    })
    //set tên
    $('.note-icon-link').click(function () {
        $('.note-link-btn').val('Chèn');
        $('.modal-title').text('Chèn Link');
    })
    $('.close').click(function () {
        $('.note-modal').modal('hide');
    })
    //các chức năng trong toolbar cần tắt
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    //phải login trước khi bình luận bài viết
    $(".note-editing-area,.request_login").click(function (ev) {
        ev.preventDefault();
        $('#create_submit_comment').removeAttr('hidden');
        $.get("/User/UserLogged", {},
            function (isLogged, textStatus, jqXHR) {
                if (!isLogged) {
                    //gọi action đăng nhập khi người dùng bấm thanh toán mà chưa đăng nhập hệ thống
                    bootbox.confirm({
                        message: "Please sign in to use this function!",
                        buttons: {
                            confirm: {
                                label: 'Sign In',
                                className: 'btn-info'
                            },
                            cancel: {
                                label: 'Back',
                                className: 'btn-secondary'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                window.location = "/User/SignIn";
                            }
                        }
                    });
                }
            },
            "json"
        );
    });
    //thêm bình luận
    $('#create_submit_comment').click(function () {
        var com_content = $("#comment__con").val();
        if (com_content == "") {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2000,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Please input comment content'
            })
            return false;
        }
        else if (com_content.length < 20) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Comment content has at least 20 characters'
            })
            return false;
        }
        else if (com_content.length > 500) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Comment content do not more than 500 characters'
            })
            return false;
        }
        else {
            var data = $("#create_comment_post").serialize();
            $.ajax({
                type: "GET",
                url: "/UserSong/CommentSong",
                data: data,
                success: function (result) {
                    if (result == true) {
                        var find_text = $('#create_comment_post')
                        find_text.find('.note-editable').text('')
                        $('#create_comment_post').val('')
                        $('.append__cmt_create').removeAttr('hidden')
                        $('.append__cmt_create').text('You comment is in censorship progress. Please wait seconds.')
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'top',
                            showConfirmButton: false,
                            timer: 2000,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })
                        Toast.fire({
                            icon: 'success',
                            title: 'Comment successfully'
                        })
                    }
                    else {
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'top',
                            showConfirmButton: false,
                            timer: 1000,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })
                        Toast.fire({
                            icon: 'error',
                            title: 'Error'
                        })
                    }
                }
            })
        };
    });
});
//parent reply comment post
var Create_Reply_comm = function (id, acc_name) {
    ide = id;
    $('.append__cmt').text('')
    $('#areas_reply_comment_' + id + '').collapse('show')
    $('#areas_reply_comment_' + id + '').collapse('hide')
    $('#submit_reply_comm_' + id + '').removeAttr('hidden');
    $('#areas_reply_comment_' + id + '').removeAttr('hidden');
    $('#areas_reply_comment_' + id + '').addClass('mt-2');
    $('#reply_comment_con_' + id + '').text("@" + acc_name + ":");
    $('#reply_comment_con_' + id + '').summernote({
        height: 100,
        placeholder: 'Leave your comment here...',
        hint: {
            words: [acc_name],
            match: /\b(\w{1,})$/,
            search: function (keyword, callback) {
                callback($.grep(this.words, function (item) {
                    return item.indexOf(keyword) === 0;
                }));
            }
        }
    });
    //ản các chức năng trong toolbar
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    //gửi reply bình luận
    $('#submit_reply_comm_' + ide + '').click(function () {
        var _reply_content = $('#reply_comment_con_' + ide + '').val();
        if (_reply_content == "") {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Please input comment content!'
            })
        }
        else if (_reply_content.length < 20) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Comment content has at least 20 characters.'
            })
            return false;
        }
        else if (_reply_content.length > 500) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Comment content do not more than 500 characters'
            })
            return false;
        }
    });
}

//reaction comment post
var Like_Comment = function (cmt_id, count_like_cmt) {
    var id = cmt_id
    var count_like = count_like_cmt
    $.ajax({
        type: "POST",
        url: '/UserSong/LikeSongComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Song_comment_id: id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Remove_Like_Comment(' + id + ',' + (count_like + 1) + ')' + '');
                $('#BtnReacton_' + id + '').text('Dislike');
                $('#sumlike_cmt_' + id + '').text('(' + (count_like + 1) + ')');
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Error'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'danger',
                title: 'Error'
            })
        }
    });
}
// remove reaction comment post
var Remove_Like_Comment = function (cmt_id, count_like_cmt) {
    var id = cmt_id
    var count_like = count_like_cmt
    $.ajax({
        type: "POST",
        url: '/UserSong/LikeSongComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Song_comment_id: id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Like_Comment(' + id + ',' + (count_like - 1) + ')' + '');
                $('#BtnReacton_' + id + '').text('Like');
                $('#sumlike_cmt_' + id + '').text('(' + (count_like - 1) + ')');
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Error'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'danger',
                title: 'Error'
            })
        }
    });
}

//reaction on song
var Love_Song = function (s_id, count_love_song) {
    var id = s_id
    var count_like = count_love_song
    $.ajax({
        type: "POST",
        url: '/UserSong/SongLoveReact',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Song_id: id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Remove_Like_Comment(' + id + ',' + (count_like + 1) + ')' + '');
                $('#BtnReacton_' + id + '').text('Remove Love');
                $('#sumlike_cmt_' + id + '').text('(' + (count_like + 1) + ')');
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Error'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'danger',
                title: 'Error'
            })
        }
    });
}
// remove reaction comment post
var Remove_Like_Comment = function (s_id, count_like_cmt) {
    var id = s_id
    var count_like = count_like_cmt
    $.ajax({
        type: "POST",
        url: '/UserSong/SongLoveReact',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Song_id: id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Like_Comment(' + id + ',' + (count_like - 1) + ')' + '');
                $('#BtnReacton_' + id + '').text('Love');
                $('#sumlike_cmt_' + id + '').text('(' + (count_like - 1) + ')');
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Error'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'danger',
                title: 'Error'
            })
        }
    });
}