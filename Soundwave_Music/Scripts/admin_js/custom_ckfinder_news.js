$("#selectnewsimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linknewsimage").val(fileurl);
    };
    finder.popup();
});