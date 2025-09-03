 google.charts.load('50', {
    callback: DrawCumulativeChart,
    packages: ['corechart']
});
function DrawCumulativeChart() {
    $('#loading').show();
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    if (report.trim() == '') {
        startDate = ExamStartDate.ExamStartDate;
        endDate = ExamCompletedDate.ExamCompletedDate;
    }
    else {
        var dateParts = report.split(' To ');
        startDate = dateParts[0].trim();
        endDate = dateParts[1].trim();
    }
    var subspecialtyId = Id.SubSpecialityId;
    $('.loader_main').fadeIn(100);
    var jsonData = $.ajax({
        type: 'POST',
        url: '/ResidentChapterReport/ResidentChartDetail'+'?subspecialtyId=' + subspecialtyId + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate,
        dataType: "json",
        //data: { subspecialtyId: subspecialtyId, examStartDate: startDate, examCompletedDate: endDate },
        cache: false,
        success: function (result) {
            $("#ResidentReportChartDevice").empty();
            var data = new google.visualization.DataTable();
            $('#loading').hide();
            var ResidenttrHtmlrow = '';
            data.addColumn('string', 'ChapterName');
            data.addColumn('number', 'Correct');
            data.addColumn('number', 'Incorrect');
            data.addColumn('number', 'Remaining');
            data.addColumn({ name: 'Total', type: 'string', role: 'annotation' });
            $.each(result, function (key, value) {
                data.addRow([value.ChapterName, value.Correct, value.InCorrect, value.RemainingQuestion, '']);
                ResidenttrHtmlrow += '<tr><td data-title="Chapter">' + value.ChapterName + '</td><td data-title="Correct" class="font-green">' + value.Correct + '</td><td data-title="Incorrect" class="font-red">' + value.InCorrect + '</td></tr>';
                
            })
            $('#ResidentReportChartDevice').append(ResidenttrHtmlrow);

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
                }  //,
               // {
               //     calc: function (dt, row) {
               //         return 0;
               //     },
               //     label: "Total",
               //     type: "number",
               //} //,
                //{
                //    calc: function (dt, row) {
                //        return dt.getValue(row, 1) + dt.getValue(row, 2) + dt.getValue(row, 3);
                //    },
                //    type: "number",
                //    role: "annotation"
                //}
            ]);
            var options ={ title: "Chapters", titleTextStyle:{ italic:false },
                animation: {
                    duration: 1000,
                    easing: 'out',
                    startup: true
                },
                'chartArea': { 'width': '60%', 'height': '90%' },
                scroll: true,
                backgroundColor: 'transparent',
                hAxis: { title: "Number of Questions",titleTextStyle: {italic: false}},
                colors: ['#a9c23f', '#d05a57', '#ddddde', '#f15a22'],
                backgroundColor: { fill: '#ffffff' },
                height: '1000',
                width: '100%',
                legend: { position: 'none' },
                bar: { groupWidth: '70%', groupHeight: '60%' },
                fontSize: 12,
                isStacked: true,
                //series: {
                //    3: {
                //        annotations: {
                //            stem: {
                //                color: "#000000",
                //                length: 28,
                //            },
                //            textStyle: {
                //                color: "#000000",
                //            }
                //        },
                //        enableInteractivity: false,
                //        tooltip: "none",
                //        visibleInLegend: false,
                //    }
                //}
            };
            var chart = new google.visualization.BarChart(document.getElementById('ResidentSectionChapter_chart'));
            chart.draw(view, options);
            $('.loader_main').fadeOut(100);
        }
    });
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

});
$('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    DrawCumulativeChart();

});
$("#Export").on('click', function (event) {

    var subspecialtyId = Id.SubSpecialityId;
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
    window.location.href ='/ResidentChapterReport/ExportToExcelGraph' +'?subspecialtyId=' + subspecialtyId + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;

});


