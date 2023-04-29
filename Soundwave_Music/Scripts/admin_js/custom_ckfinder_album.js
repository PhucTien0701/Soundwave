$("#selectalbumimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkalbumimage").val(fileurl);
    };
    finder.popup();
});