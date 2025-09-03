google.charts.load('50', {
    callback: DrawCumulativeChart,
    packages: ['corechart']
});

function DrawCumulativeChart() {
    var startDate;
    var endDate;
    var pgyYear;
    var report = $('#reportrange').text();
    if (report.trim() == '' || report.trim() =="Select Date Range") {
        startDate =ExamStartDate.ExamStartDate;
        endDate = ExamCompletedDate.ExamCompletedDate;
    }
    else {
        var dateParts = report.split(' To ');
        startDate =dateParts[0].trim();
        endDate =dateParts[1].trim();
    }
    var subspecialtyId = Id.SubSpecialityId;
  
    if (pgyYear == '') {
        pgyYear = Year.Year;
    }
    else {
        if ($('#pgyYear option:selected').text() == 'All' || $('#pgyYear option:selected').text() == 'Non-Resident') {
            pgyYear = $('#pgyYear option:selected').val();
        }
        else {
            pgyYear = $('#pgyYear option:selected').text();
        }
    }
    $('.loader_main').fadeIn(100);
    var jsonData = $.ajax({
        type: 'POST',
        url: '/AdminChapterReport/AdminChartDetail'+'?year='+pgyYear+'&subspecialtyId='+subspecialtyId+'&examStartDate='+startDate+'&examCompletedDate='+endDate,
        dataType: "json",
      //  data: { subspecialtyId: subspecialtyId, examStartDate: startDate, examCompletedDate: endDate },
        cache: false,
        success: function (result) {         
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'ChapterName');
            data.addColumn('number', 'correct');
            data.addColumn('number', 'Incorrect');
            data.addColumn('number', 'Remaining');
            data.addColumn({ name: 'Total', type: 'string', role: 'annotation' });
            $.each(result, function (key, value) {
                data.addRow([value.ChapterName, value.Correct, value.InCorrect, value.RemainingQuestion, '']);
            })

            var view = new google.visualization.DataView(data);
            view.setColumns([0,
                1, {
                    calc: function (dt, row) {
                        return dt.getValue(row, 1);
                    },
                    type: "number",
                    role: "annotation"
                },
                2, {
                    calc: function (dt, row) {
                        return dt.getValue(row, 2);
                    },
                    type: "number",
                    role: "annotation"
                },

                3, {
                    calc: function (dt, row) {
                        return dt.getValue(row, 3);
                    },
                    type: "number",
                    role: "annotation"
                },
                //{
                //    calc: function (dt, row) {
                //        return 0;
                //    },
                //    label: "Total",
                //    type: "number",
                //},
                //{
                //    calc: function (dt, row) {
                //        return dt.getValue(row, 1) + dt.getValue(row, 2) + dt.getValue(row, 3);
                //    },
                //    type: "number",
                //    role: "annotation"
                //}
               
            ]);
        
            var options = {
                title: "Performance By Chapter", titleTextStyle: { italic: false, textalign: "center" },
                animation: {
                    duration: 1000,
                    easing: 'out',
                    startup: true
                },
                'chartArea': { 'width': '60%', 'height': '90%' },
                backgroundColor: 'transparent',
                hAxis: { },
                colors: ['#a9c23f', '#d05a57', '#ddddde', '#f15a22'],
                backgroundColor: { fill: '#ffffff' },
                height: '800',
                width: '100%',
                legend: { position: 'none' },
                bar: { groupWidth: '70%', groupHeight: '60%' },
                fontSize: 12,
                isStacked: true,
                //series: {
                //    3: {
                //        annotations: {
                //            stem: {
                //                color: "transparent",
                //                length: 28
                //            },
                //            textStyle: {
                //                color: "#000000",
                //            }
                //        },
                //        enableInteractivity: false,
                //        tooltip: "none",
                //        visibleInLegend: false
                //    }
                //}
            };

            var chart = new google.visualization.BarChart(document.getElementById('AdminSectionChapter_chart'));
            chart.draw(view, options);
        
           // myChart.Series["chart"].ChartType = SeriesChartType.Column;
        }
    });
    //if (Year.Year == 'All' || Year.Year == 'Non-Resident') {
        
    //}
   // PGYYear
    //$('#pgyYear').empty();
    //$('#pgyYear').attr("disabled", "disabled");
    //$('#pgyYear').append($("<option></option>").attr("value", Year.Year).text(Year.Year));
}
$(window).resize(function () {
    DrawCumulativeChart();
});

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
            $("#pgyYear").html(pgyYear);

        }
    });



    $('#pgyYear').on('change', function () {
        var startDate;
        var endDate;
        var report = $('#reportrange').text();
        if (report.trim() == '') {
            startDate = ExamStartDate.ExamStartDate;
            endDate = ExamCompletedDate.ExamCompletedDate;
        }
        else {
            var dateParts = report.split('To');
            startDate = dateParts[0];
            endDate = dateParts[1];

        }
        var year;
        if ($('#pgyYear option:selected').text() == 'All' || $('#pgyYear option:selected').text() == 'Non-Resident') {
            year = $('#pgyYear option:selected').val();
        }
        else {
            year = $('#pgyYear option:selected').text();
        }
        DrawCumulativeChart();

    });
    //$('#pgyYear').val(Year.Year);
  
});
$('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    DrawCumulativeChart();

});
$("#Export").on('click', function (event) {

    var subspecialtyId = Id.SubSpecialityId;
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    var pgyYear;
    if (pgyYear == '') {
        pgyYear = Year.Year;
    }
    else {
        if ($('#pgyYear option:selected').text() == 'All' || $('#pgyYear option:selected').text() == 'Non-Resident') {
            pgyYear = $('#pgyYear option:selected').val();
        }
        else {
            pgyYear = $('#pgyYear option:selected').text();
        }
    }
    if (report.trim() == '') {
        startDate = ExamStartDate.ExamStartDate;
        endDate = ExamCompletedDate.ExamCompletedDate;

    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];
    }
    window.location.href ='/AdminChapterReport/ExportToExcelGraph'+'?year='+pgyYear+'&subspecialtyId='+subspecialtyId+'&examStartDate='+startDate+'&examCompletedDate='+endDate;

});

