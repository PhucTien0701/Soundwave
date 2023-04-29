$("#selectsupplierimg").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linksupplierimage").val(fileurl);
    };
    finder.popup();
});