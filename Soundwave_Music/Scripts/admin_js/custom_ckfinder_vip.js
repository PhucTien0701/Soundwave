$("#selectvipimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkvipimage").val(fileurl);
    };
    finder.popup();
});