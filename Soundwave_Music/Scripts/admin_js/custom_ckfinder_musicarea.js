$("#selectareaimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkareaimage").val(fileurl);
    };
    finder.popup();
});