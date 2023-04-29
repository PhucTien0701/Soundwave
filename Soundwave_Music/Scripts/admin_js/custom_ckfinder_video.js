$("#selectvideo").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkvideo").val(fileurl);
    };
    finder.popup();
});