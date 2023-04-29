$("#selectgenreimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkgenreimage").val(fileurl);
    };
    finder.popup();
});