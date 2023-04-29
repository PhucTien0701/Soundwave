$("#selectcomposerimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkcomposerimage").val(fileurl);
    };
    finder.popup();
});