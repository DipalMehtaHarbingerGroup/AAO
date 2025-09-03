$(document).ready(function () { 
    var start = moment().subtract(29, 'days');
    var end = moment();
    function cb(start, end) {
        $('#reportrange span').html(start.format('MM-DD-YYYY') + ' To ' + end.format('MM-DD-YYYY'));
    }
    cb(start, end);
    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxDate: moment(),
        ranges: {
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'Last 3 Months': [moment().subtract(90, 'days'), moment()],
            'Last 6 Months': [moment().subtract(180, 'days'), moment()],
            '1 Year': [moment().subtract(365, 'days'), moment()]
        }
    }, cb);



    var year;
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    if (report.trim() == '') {
        startDate = null;
        endDate = null;

    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];

    }

    if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        year = $('#Year option:selected').val();
    }
    else {
        year = $('#Year option:selected').text();
    }

    FilterByDate(year, startDate, endDate);


    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        var year;
        var startDate;
        var endDate;

        if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
            year = $('#Year option:selected').val();
        }
        else {
            year = $('#Year option:selected').text();
        }
        var report = $('#reportrange').text();
        if (report.trim() == '') {
            startDate = null;
            endDate = null;

        }
        else {
            var dateParts = report.split('To');
            startDate = dateParts[0];
            endDate = dateParts[1];

        }

        $(FilterByDate(year, startDate, endDate));
    });

    
    var pgyYear = "";
    $.ajax({
        type: 'GET',
        url: '/AggregateUserPerformance/GetPYGYear',
        success: function (result) {
            pgyYear = '<option value="-1">' + "All" + '</option>';
            for (var i = 0; i < result.length; i++) {
               // pgyYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
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
            var startDate;
            var endDate;
            var report = $('#reportrange').text();
            if (report.trim() == '') {
                startDate = null;
                endDate = null;

            }
            else {
                var dateParts = report.split('To');
                startDate = dateParts[0];
                endDate = dateParts[1];

            }
           var year;
            if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
                year = $('#Year option:selected').val();
            }
            else {
                year = $('#Year option:selected').text();
            }
            $(FilterByDate(year, startDate, endDate));
    });
});


function FilterByDate(year, startDate, endDate) {

    var startDate = startDate;
    var endDate = endDate;
    var pgyYear=year;
    
    $('#AdminReport').dataTable().fnDestroy();
    $('#AdminReport').DataTable({
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
          //  "data": { "year": pgyYear, "examStartDate": startDate, "examCompletedDate": endDate },
            "type": "POST",
            "url":'/AggregateUserPerformance/AdminReportDetails' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate,
        },
        "columns": [
            {
               
                "data": null, "bSortable": true, "name": "Subspecialty_Name" ,render: function (data, type, row) {
                    return '<a href="/AdminChapterReport/AdminChapterReportDetails?year='+ pgyYear + '&SubspecialtyId=' + data.SubspecialtyId + '&ExamStartDate=' + startDate + '&ExamCompletedDate=' + endDate + '" class="editAsset" > ' + 'Section ' + data.BCSCSectionNumber + ': '+ data.SubspecialtyName + '</a>';
                },
               
            },

            { "data": "Correct", "bSortable": true, "name": "Correct" },
            {
                data: null, "bSortable": true, "name": "InCorrect" , render: function (data, type, row) {
                    if (data.InCorrect == 0) {
                        return 0;
                    }
                    else {

                        return '<a href="/IncorrectAdminReport/AdminIncorrectQuestionDetails?year='+ pgyYear + '&SubspecialtyId=' + data.SubspecialtyId + '&ExamStartDate=' + startDate + '&ExamCompletedDate=' + endDate + '" class="editAsset">' + data.InCorrect + '</a>';
                    }
                }
            },
            {
                "data": null, "bSortable": true, "name": "Score" , render: function (data, type, row) {

                    return data.Score + '%';
                }
            },
            { "data": "UserId", "bSortable": true, "name": "UserId"   },
        ],
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
     window.location.href = '/AggregateUserPerformance/ExportToExcel' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;


});

