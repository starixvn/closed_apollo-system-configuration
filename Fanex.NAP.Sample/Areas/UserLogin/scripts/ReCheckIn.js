function ReCheckin() {
    var KickedOut = 3;
    var strOnUserId = $("#hdfOnUserId").val();

    $.ajax({
        url: location.protocol + '//' + location.host + "/UserLogin/Login/ReCheckLogin",
        type: "POST",       
        dataType: "json",
        success: function (result) {
            if (result == "1") {
                window.location.href = location.protocol + '//' + location.host + "/UserLogin/Login/LogOut";
            }
        },
        error: function (request, status, error) {
        }
    });
}

function closeBrowser() {
    var index = window;
    while (index != index.parent) // Jump out
    {
        index = index.parent;
    }

    index.close();
}

$(document).ready(function () {
    setInterval("ReCheckin()", 10000);
});