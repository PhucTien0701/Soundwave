$("#selectsingerimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linksingerimage").val(fileurl);
    };
    finder.popup();
});