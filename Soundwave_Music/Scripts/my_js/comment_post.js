$(document).ready(function () {
    $('.form_comment_post').summernote({
        height: 100,
        placeholder: 'Leave your comment here...',
    });
    $('.note-icon-picture').click(function () {
        $('.note-image-btn').val('Insert');
        $('.modal-title').text('Insert image');
        $('.note-group-select-from-files .note-form-label').text('Choose file');
        $('.note-group-image-url .note-form-label').text('image URL');
    })
    $('.note-icon-link').click(function () {
        $('.note-link-btn').val('Insert');
        $('.modal-title').text('Insert Link');
    })
    $('.close').click(function () {
        $('.note-modal').modal('hide');
    })
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    $(".note-editing-area,.request_login").click(function (ev) {
        ev.preventDefault();
        $('#create_submit_comment').removeAttr('hidden');
        $.get("/User/UserLogged", {},
            function (isLogged, textStatus, jqXHR) {
                if (!isLogged) {
                    bootbox.confirm({
                        message: "Please login to use this action!",
                        buttons: {
                            confirm: {
                                label: 'Login',
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
                title: 'Please input comment content.'
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
                title: 'The comment content has at least 20 characters'
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
                title: 'The comment content is no more than 20 characters'
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
                        $('.append__cmt_create').text('Your comment will be displayed after approved.')
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
                            title: 'comment successfully'
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
                $('#BtnReacton_' + id + '').attr('onclick', 'Remove_Like_Comment(' + id + ',' + (count_like + 1)+ ')' + '');
                $('#BtnReacton_' + id + '').text('Dislike');
                $('#sumlike_cmt_' + id + '').text('('+(count_like+1)+')');
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
                $('#sumlike_cmt_' + id + '').text('('+(count_like - 1)+')');
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