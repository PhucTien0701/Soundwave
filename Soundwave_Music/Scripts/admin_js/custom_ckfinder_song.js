$("#selectaudio").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkaudio").val(fileurl);
    };
    finder.popup();
});