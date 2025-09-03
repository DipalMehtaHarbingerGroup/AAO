
$(document).ready(function () {
    $.ajax({
        type: 'Post',
        dataType: "JSON",
        url: '/CMECredit/GetCMECredit',
        success: function (result) {
            if (result != '') {
                $.each(result, function (key, value) {
                    var data = "<tr><td> Section " + value.BCSCSectionNumber + ": " + value.SubSpecialityName + "</td>"
                    if (value.AttemptedCount >= 15) {
                        data += "<td><button type='button' id='button" + value.BCSCSectionNumber + "' class='btn btn-primary cmecreditbtn' data_URL=" + value.CMECreditPath + ">Claim</button></td></tr>"
                    }
                    else {
                        data += "<td><button type='button' disabled='disabled' class='btn btn-primary' onclick='URLopen()'>Claim</button></td></tr>"
                    }
                    $('#CMECreditTable').append(data);
                });
            }
            BindEvent();
        }
    });
});
   

  function BindEvent() {
    $(".cmecreditbtn").on("click", function () {     
        var dataURL = $(this).attr("data_URL");
        window.open(dataURL);
    });
}


