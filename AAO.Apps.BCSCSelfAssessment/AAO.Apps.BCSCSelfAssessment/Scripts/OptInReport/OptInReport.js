$(document).ready(function () {
    
    var year;

    if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        year = $('#Year option:selected').val();
    }
    else {
        year = $('#Year option:selected').text();
    }

    FilterByDate(year);


    var pgyYear = "";
    $.ajax({
        type: 'GET',
        url: '/AggregateUserPerformance/GetPYGYear',
        success: function (result) {
            pgyYear = '<option value="-1">' + "All" + '</option>';
            for (var i = 0; i < result.length; i++) {
                if (result[i].PGYYear == 'Non-Resident') {
                    pgyYear += '<option value=-2>' + result[i].PGYYear + '</option>';
                } else {
                    pgyYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
                }
            }
            $("#Year").html(pgyYear);

        }
    });



    $('#Year').on('change', function () {
        var year;
        if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
            year = $('#Year option:selected').val();
        }
        else {
            year = $('#Year option:selected').text();
        }
        $(FilterByDate(year));
    });
});


function FilterByDate(year) {

    var pgyYear = year;

    $('#OptInReport').dataTable().fnDestroy();
    $('#OptInReport').DataTable({
        "aaSorting": [[0, 'asc'], [1, 'asc']],
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderable": true,
        "language": {
            "lengthMenu": "Show _MENU_ exams",
            "sEmptyTable": "No Records found",
            "info": "",

        },
        "paging": false,
        "scrollX": false,
        "ajax": {
            "type": "POST",
            "url": '/OptInReports/OptInReports' + '?year=' + pgyYear,
        },
        "columns": [


            { "data": "UserName", "bSortable": true, "name": "UserName" },
            { "data": "MasterCustomerID", "bSortable": true, "name": "MasterCustomerID" },
            { "data": "OptIn", "bSortable": true, "name": "OptIn" },
        ],
    });

    ataglance(pgyYear);
}
function ataglance(year) {
    $.ajax({
        url: '/OptInReports/GetOptIn',
        type: 'POST',
        data: { year: year},
        success: function (result) {
            $('#TotalUserCount').text(result.TotalUserCount);
            $('#TotalAcceptedUserCount').text(result.TotalOptInAcceptCount);
            $('#TotalDeclineUserCount').text(result.TotalOptInDeclineCount);
        }
    });

}

$("#Export").on('click', function (event) {
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    var pgyYear;

    if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        pgyYear = $('#Year option:selected').val();
    }
    else {
        pgyYear = $('#Year option:selected').text();
    }

    if (report.trim() == '') {
        startDate = null;
        endDate = null;
    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];
    }
    window.location.href = '/OptInReports/ExportToExcel' + '?year=' + pgyYear;


});

