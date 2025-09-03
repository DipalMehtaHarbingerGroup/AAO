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

    if ($('#PGYear option:selected').text() == 'All' || $('#PGYear option:selected').text() == 'Non-Resident') {
        year = $('#PGYear option:selected').val();
    }
    else {
        year = $('#PGYear option:selected').text();
    }

    FilterByDate(year, startDate, endDate);


    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        var year;
        var startDate;
        var endDate;

        if ($('#PGYear option:selected').text() == 'All' || $('#PGYear option:selected').text() == 'Non-Resident') {
            year = $('#PGYear option:selected').val();
        }
        else {
            year = $('#PGYear option:selected').text();
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
        $(FilterByDate(year, startDate, endDate));
    });
   
  var pgyYear = "";
    $.ajax({
        type: 'GET',
        url: '/AggregateUserPerformance/GetPYGYear',
        success: function (result) {
            pgyYear = '<option value="-1">' + "All" + '</option>';
            for (var i = 0; i < result.length; i++) {
                //pgyYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
                if (result[i].PGYYear == 'Non-Resident') {
                    pgyYear += '<option value=-2>' + result[i].PGYYear + '</option>';
                } else {
                    pgyYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
                }
            }
            $("#PGYear").html(pgyYear);

        }
    });

  $('#PGYear').on('change', function () {
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
        if ($('#PGYear option:selected').text() == 'All' || $('#PGYear option:selected').text() == 'Non-Resident') {
            year = $('#PGYear option:selected').val();
        }
        else {
            year = $('#PGYear option:selected').text();
        }
        refreshTable();
        $(FilterByDate(year, startDate, endDate));
    });
   
});
function refreshTable() {
    $('#QuestionPerformanceReport').each(function () {
      dt = $(this).dataTable();
      dt.fnDraw();
  })
}

function FilterByDate(year, startDate, endDate) {
    var startDate;
    var endDate;
    var pgyYear = year;

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
    $('#QuestionPerformanceReport').dataTable().fnDestroy();
    var QuestionReport = $('#QuestionPerformanceReport').dataTable({
        "ajax": {
            "type": "POST",
            "url": '/AdminQuestionPerformance/AdminQuestionPerformanceDetails'+'?year='+pgyYear+'&examStartDate='+startDate+'&examCompletedDate='+endDate,
        },
        "lengthMenu": [[25, 50, 75, 100], [25, 50, 75, 100]],
        "language": {
            "processing": "<img src='/Content/img/Processing.gif'> Loading...",
            "lengthMenu": "Show _MENU_ ",
            "sEmptyTable": "No Records found",
            "info": "Showing _START_ - _END_ of _TOTAL_ entries",

        },
        "paging": true,
        "processing": true,
        "serverSide": true,
        "filter": false,
        
         "columns": [
             { "data": "Stem", "bSortable": true, "name": "Stem" },
             { "data": "QuestionId", "bSortable": true, "name": "QuestionId"},
             { "data": "Subspecialty", "bSortable": true, "name": "Subspecialty" },
             { "data": "Chapter", "bSortable": true, "name": "Chapter" },
             { "data": "Topic", "bSortable": true, "name": "Topic"  },
             { "data": "UserAnsweredcorrectly", "bSortable": true, "name": "UserAnsweredcorrectly" },
             { "data": "UserAnsweredincorrectly", "bSortable": true, "name": "UserAnsweredincorrectly" },
             { "data": "UsersRepeatedSRMode", "bSortable": true, "name": "UsersRepeatedSRMode" },
            {
                data: null, "bSortable": true, "name": "Percentagecorrectly" ,render: function (data, type, row) {
                    return data.Percentagecorrectly + '%';
                }
            },
            {
                data: null, "bSortable": true, "name": "PercentageIncorrectly" , render: function (data, type, row) {
                    return data.PercentageIncorrectly + '%';
                }
            },
            { "data": "CorrectAnswer", "bSortable": true ,"name": "CorrectAnswer"},
            { "data": "Distractor_1", "bSortable": true,"name": "Distractor_1"  },
            { "data": "User_chose_distractor_1", "bSortable": true,"name": "User_chose_distractor_1"   },
            { "data": "Distractor_2", "bSortable": true,"name": "Distractor_2"  },
            { "data": "User_chose_distractor_2", "bSortable": true ,"name": "User_chose_distractor_2"  },
            { "data": "Distractor_3", "bSortable": true,"name": "Distractor_3"   },
            { "data": "User_chose_distractor_3", "bSortable": true,"name": "User_chose_distractor_3"  },
            { "data": "Avg_times_incorrect_before_correct", "bSortable": true, "name": "Avg_times_incorrect_before_correct" },
            { "data": "Users_correct_first_try", "bSortable": true, "name": "Users_correct_first_try" }
        ],
    });
   
}
$("#Export").on('click', function (event) {
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    var pgyYear;

    if ($('#PGYear option:selected').text() == 'All' || $('#PGYear option:selected').text() == 'Non-Resident') {
        pgyYear = $('#PGYear option:selected').val();
    }
    else {
        pgyYear = $('#PGYear option:selected').text();
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
    window.location.href = '/AdminQuestionPerformance/ExportToExcel' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;

});
