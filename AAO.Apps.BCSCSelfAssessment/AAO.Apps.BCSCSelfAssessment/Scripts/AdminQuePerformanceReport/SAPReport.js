$(document).ready(function () {
    $("#ExportAll").on('click', function (event) {
        var start = moment().subtract(29, 'days');
        var end = moment();
        function cb(start, end) {
            $('#reportrangeModel span').html(start.format('MM-DD-YYYY') + ' To ' + end.format('MM-DD-YYYY'));
        }
        cb(start, end);
        $('#reportrangeModel').daterangepicker({
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

        $('#ExamTimeType').prop('checked', false);

        var startDate;
        var endDate;
        var report = $('#reportrangeModel').text();
        if (report.trim() == '') {
            startDate = null;
            endDate = null;

        }
        else {
            var dateParts = report.split('To');
            startDate = dateParts[0];
            endDate = dateParts[1];

        }

        var pgYear = "";
        $.ajax({
            type: 'GET',
            url: '/AggregateUserPerformance/GetPYGYear',
            success: function (result) {
                pgYear = '<option value="-1">' + "All" + '</option>';
                for (var i = 0; i < result.length; i++) {
                    //pgyYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
                    if (result[i].PGYYear == 'Non-Resident') {
                        pgYear += '<option value=-2>' + result[i].PGYYear + '</option>';
                    } else {
                        pgYear += '<option value="' + i.toString() + '">' + result[i].PGYYear + '</option>';
                    }
                }
                $("#PGYearModel").html(pgYear);

            }
        });
    });


    $("#ExportQuestions").on('click', function (event) {
        var OptinValue = $('#ExamTimeType').prop('checked');
        var startDate;
        var endDate;
        var report = $('#reportrangeModel').text();
        var pgYear;

        if ($('#PGYearModel option:selected').text() == 'All' || $('#PGYearModel option:selected').text() == 'Non-Resident') {
            pgYear = $('#PGYearModel option:selected').val();
        }
        else {
            pgYear = $('#PGYearModel option:selected').text();
        }

        if (report.trim() == '') {
            startDate = null;
            endDate = null;
        }
        else {
            var dateParts = report.trim().split('To');
            startDate = dateParts[0];
            endDate = dateParts[1];
        }
        window.location.href = '/AdminQuestionPerformance/ExportAllQuestionToExcel' + '?year=' + pgYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate + '&optOutValue=' + OptinValue;
    });
}); /*();*/