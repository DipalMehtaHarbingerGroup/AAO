//google.charts.load('current', {
//    callback: DrawCumulativeChart,
//    packages:['corechart']
//});

google.charts.load({ packages: ["corechart", "gauge", "table", "timeline", "bar"] });
google.charts.setOnLoadCallback(DrawCumulativeChart);

$(function () {
    // start Call chart on Peer Comparision  change
    $('#toggle-switch1').change(function () {
        if ($('#toggle-switch1').is(":checked")==true) {
            $('#CommulativePerformance_chart').html('');
            $("#OnChangeFilterChartId").show();
            $("#DefaultFilterChartId").hide();
            DrawCumulativeChart();

        } else {
            $('#CommulativePerformance_chart').html('');
            $("#DefaultFilterChartId").show();
            $("#OnChangeFilterChartId").hide();
            DrawCumulativeChart();
        }
    });
    // end Call cahrt on Peer Comparision  change

    // start Call cahrt on Exam type change
    $('#ChartTypeId').change(function () {
        if ($('#toggle-switch1').is(":checked") == true) {
            $('#CommulativePerformance_chart div').html('');
            DrawCumulativeChart();
        } else {
            $('#CommulativePerformance_chart div').html('');
            DrawCumulativeChart();
        }
    });
    // end Call cahrt on Exam type change
   
    // For QuickExam from dashboard
    $(document).on('click', "#examBuildquick", function () {
        console.log('examBuildquick');
        window.location.href = '/ExamManager/Index#QuickExam';
    })

    // start Call cahrt on date range change
    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        if ($('#toggle-switch1').is(":checked") == true) {
            $('#CommulativePerformance_chart div').html('');
            DrawCumulativeChart();
        } else {
            $('#CommulativePerformance_chart div').html('');
            DrawCumulativeChart();
        }
    });
    // end Call cahrt on Exam type change
        //var start = moment().subtract(365, 'days'); //StartDate = past 1 year from current date
        var StartDate = '06/01/2018' // Start date changes to '1 June 2018' 
        var start = moment(StartDate);
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
                '1 Year': [moment().subtract(365, 'days'), moment()],

            }
        }, cb);


    //Exam History Code

    $.ajax({
        type: 'Post',
        dataType: "JSON",
        url: '/Dashboard/GetExamStatusCount',
        success: function (result) {
            $(".exams-all").append(result.allExam);
            $(".exams-completed").append(result.completed);
            $(".exams-inprogress").append(result.inProgress);
        }
    });

    $("#ExamHistoryPageRedirect").on('click', function () {
        window.location.href = '/ExamHistory/Index';
    });
    // End Of History Code


    //Exam Average Score
    $.ajax({
        type: 'Post',
        dataType: "JSON",
        url: '/Dashboard/GetExamScore',
        success: function (result) {
            var resdata = result;
            $.each(result, function (key, value) {
                if (value.examType == "1") {
                    $(".simulatedAvgScore").append(value.averageScore +"%");
                    $(".simulaterdAttemptQuestion").append(value.totalAttempedQuestion);
                    $(".simulatedCorrectAnswer").append(value.correctAnswer);
                    $(".simulatedIncorrectAnswer").append(value.inCorrectAnswer);
                }
                else if (value.examType == "0") {
                    $(".customAvgScore").append(value.averageScore+"%");
                    $(".customAttemptQuestion").append(value.totalAttempedQuestion);
                    $(".customCorrectAnswer").append(value.correctAnswer);
                    $(".customInCorrectAnswer").append(value.inCorrectAnswer);
                }
            });
        }
    });
    // End of Exam Average Score

    // Exam Progress List
    function loadData() {
        $.ajax({
            type: 'Post',
            dataType: "JSON",
            url: '/Dashboard/MyGetmyExamHistoryList',
            success: function (result) {
                if (result != '') {
                    $.each(result, function (key, value) {
                        if (value.ExamId > 0) {
                            var data = "<tr><td class=exam-title>" + value.ExamName + "  (" + value.AttempedQuestionCount + "/" + value.TotalQuestionCount + ")</td>"
                            data += "<td class='fainted-font examfont1'>" + value.ExamType + " </td>"
                            data += "<td class='fainted-font'>" + "Last attemped on " + value.LastAttempedDate + " </td>"
                            data += "<td class=text-right><button class='btn btn-primary resume' id='resumebutton' resume_exam_id=" + value.ExamId + "  resume resume_question_id=" + value.NextAttemptQuestionId + ">Resume</button></td></tr>"
                            $('#no-more-tables').append(data);
                            $("#ExamlistId1").show();
                            $("#ExamlistId2").hide();
                            $("#ExamProgreesId1").show();
                            $("#PerformanceSummeryId").show();
                            $("#ExamProgreesId2").hide();
                            $("#ScoreComparisonToggle").show(); //Added for toggle switch enable all time
                        } else {
                          
                            $("#ExamlistId1").show();
                            $("#ExamlistId2").hide();
                            $("#ExamProgreesId2").removeClass("col-lg-9");
                            $("#ExamProgreesId2").addClass("col-lg-8");                     
                            $("#ScoreComparisonToggle").show();
                            $("#PerformanceSummeryId").show();
                            $("#ExamProgreesId1").hide();
                            $("#ExamProgreesId2").show();
                        }
                    });
                }
                else {

                   // $('#authors').show();
                    //$('body').addClass('my-body-noscroll-class');
                    $("#ExamlistId1").show(); //.hide();
                    //$("#ExamlistId2").show();
                    //$("#ScoreComparisonToggle").hide(); //Commented for toggle switch enable all time
                    $("#PerformanceSummeryId").show(); //.hide();
                    $("#ExamProgreesId1").hide();
                    $("#ExamProgreesId2").show();
                    /*$("#ChartDivId").removeClass("col-lg-8");
                    $("#ChartDivId").addClass("col-lg-12");*/
                  
                }
            },
        });

        // Getting User status IsFirst User
        $.ajax({
            type: 'Post',
            dataType: "JSON",
            url: '/Dashboard/GetUserStatusIsFirst',
            async: false,
            success: function (result) {
                if (result == 0) {
                    $('#authors').show();
                    $('body').addClass('my-body-noscroll-class');
                } else {
                    GetOptInStatus();
                    //$('#authors').show(); /*Added for see content of first time user */
                }
            }
        });
        // Update User Table for IsLogFirst

        $.ajax({
            type: 'Post',
            dataType: "JSON",
            url: '/Dashboard/UpdateUserStatusIsFirst',
            success: function (result) {
            }
        });
    }
    function GetOptInStatus() {
        $.ajax({
            type: 'Post',
            dataType: "JSON",
            url: '/Dashboard/GetUserOptInStatus',
            async: false,
            success: function (result) {
                if (result == '0') {
                    $('#optInDialog').show();
                } else {
                    $('#optInDialog').fadeOut('slow');
                }
            }
        });
    }
    loadData();
    $(document).on('click', "#examBuild", function () {
        window.location.href = '/ExamManager/Index';
    })

    $(document).on('click', "#renewalLink", function () {
        var link = $(this).attr("link");
        window.location.href = link;
    })

    $(document).on('click', "#resumebutton", function () {
        var examId = $(this).attr("resume_exam_id");
        var questionId = $(this).attr("resume_question_id");
        window.location.href = '/Assessment/Index' + '?ExamId=' + examId + '&Questionid=' + questionId;

    });
    // End of Exam progress list 
    $(document).on('click', "#closeButtonNav", function () {
        $("#authors").fadeOut("slow").remove();
        $('body').removeClass('my-body-noscroll-class');
        $('#optInDialog').modal('show');
        $("#authors").remove();
    });

});

function DrawCumulativeChart() {

    var examType = $('#ChartTypeId').val();
    var toggleValue = $('#toggle-switch1').is(":checked");
    if (!toggleValue) {
        //var startDate;
        //var endDate;
        //var report = $('#reportrange').text();
        //if (report.trim() == '' || report.trim() == "Select Date Range")
        //{        
        //    startDate = null;
        //    endDate = null;      
        //}
        //else {
        //    var dateParts = report.split(' To ');      
        //    startDate = dateParts[0].trim();
        //    endDate = dateParts[1].trim();      
        //}
        $('#Chart2').hide();
        $('.loader_main').fadeIn(100);
        var startDate;
        var endDate;
        var report = $('#reportrange').text();
        if (report.trim() == '') {
            startDate = null;
            endDate = null;

        }
        else {
            var dateParts = report.split(' To ');
            startDate = dateParts[0];
            endDate = dateParts[1];
        }


        var jsonData = $.ajax({
            url: DashboardGoogleChart.url,
            type: 'GET',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: { scoreCompWithPeers: toggleValue, examType: examType, fromDate: startDate, toDate: endDate },
            cache: false,
            success: function (result) {
                $('#Chart1').empty();
                $('.loader_main').fadeOut(100);
                var trHTML = '';
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'BcscSection');
                data.addColumn('number', 'Correct');
                data.addColumn('number', 'Incorrect');
                data.addColumn('number', 'Remaining');
                data.addColumn({ name: 'Total', type: 'string', role: 'annotation' });
                $.each(result, function (key, value) {

                    //var SubSpecialityNameLink = '<a href="#">' + value.SubSpecialityName + '</a>';

                    data.addRow([value.SubSpecialityName, value.Correct, value.Incorrect, value.Remaining, '']);
                    //data.addRow([SubSpecialityNameLink, value.Correct, value.Incorrect, value.Remaining, '']);
                       
                    trHTML += '<tr><td data-title="Specialty">' + value.SubSpecialityName + '</td><td data-title="Correct" class="font-green">' + value.Correct + '</td><td data-title="Incorrect" class="font-red">' + value.Incorrect + '</td><td data-title="Remaining">' + value.Remaining + '</td><td data-title="Total">' + value.Total + '</td></tr>';
                    //trHTML += '<tr><td data-title="Specialty">' + SubSpecialityNameLink + '</td><td data-title="Correct" class="font-green">' + value.Correct + '</td><td data-title="Incorrect" class="font-red">' + value.Incorrect + '</td><td data-title="Remaining">' + value.Remaining + '</td><td data-title="Total">' + value.Total + '</td></tr>';
                })     

                $('#Chart1').append(trHTML);
                $("#Chart1").show();

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
                    {
                        calc: function (dt, row) {
                            return 0;
                        },
                        label: "Total",
                        type: "number",
                    }
                    /*,{
                        calc: function (dt, row) {
                            return dt.getValue(row, 1) + dt.getValue(row, 2) + dt.getValue(row, 3);
                        },
                        type: "number",
                        role: "annotation"
                    }*/
                ]);
                var options = {
                    animation: {
                        duration: 0,
                        easing: 'out',
                        startup: true
                    },
                    /* title: 'Exam History',*/
                    'chartArea': { 'width': '70%', 'height': '80%' },
                    backgroundColor: 'transparent',
                    // vAxis: { title: "BCSC Sectios", titleTextStyle: { italic: false } },
                    hAxis: { title: "Total Questions", titleTextStyle:{ italic: false } },
                    colors: ['#a9c23f', '#d05a57', '#ddddde', '#f15a22'],
                    backgroundColor: { fill: '#ffffff' },
                    height: 600,
                    width: '100%',
                    legend: { position: 'none' },
                    bar: { groupWidth: '60%', groupHeight: '60%' },
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
                var chart = new google.visualization.BarChart(document.getElementById('CommulativePerformance_chart'));
                chart.draw(view, options);
            }
        });
       
    } else {
        $('.loader_main').fadeIn(100);
        $('#Chart1').hide();
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
        var jsonData = $.ajax({
            url: DashboardGoogleChart.url,
            type: 'GET',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: { scoreCompWithPeers: toggleValue, examType: examType, fromDate: startDate, toDate: endDate },
            cache: false,
            success: function (result) {
                var data = new google.visualization.DataTable();
                $('.loader_main').fadeOut(100);
                $('#Chart2').empty();
              //  $('#Chart1').hide();
                var trHtmlrow2 = '';
                data.addColumn('string', 'BcscSection');
                data.addColumn('number', 'Self');
                data.addColumn('number', 'Peer');
                data.addColumn({ name: 'Total', type: 'string', role: 'annotation' });
                $.each(result, function (key, value) {
                    data.addRow([value.SubSpecialityName, value.SelfScore, value.PeerScore, '']);
                    trHtmlrow2 += '<tr><td data-title="Specialty">' + value.SubSpecialityName + '</td><td data-title="My Score" class="font-green">' + value.SelfScore + '</td><td data-title="Avg.Peer Score" class="font-red">' + value.PeerScore + '</td></tr>';
                   
                })

                $('#Chart2').append(trHtmlrow2);
                $("#Chart2").show();
              
                var view = new google.visualization.DataView(data);
                view.setColumns([0,
                                 1,
                                 {
                                     calc: "stringify",
                                     sourceColumn: 1,
                                     type: "string",
                                     role: "annotation"
                                 },
                                 2,
                                  {
                                      calc: "stringify",
                                      sourceColumn: 2,
                                      type: "string",
                                      role: "annotation"
                                  },
                ]);
                var options = {
                    width: '100%',
                    height: 900,
                    //legend: { position: 'top', maxLines: 3 },
                    //tooltip: {isHtml: true},
                    legend: 'none',
                    bar: { groupWidth: '80%', groupHeight: '80%' },
                    fontSize: 12,
                    tooltip: false,
                    //bar: { groupWidth: '90%', groupHeight: '90%' },
                    isStacked: false,
                    backgroundColor: 'transparent',
                    //     vAxis: { title: "BCSC Sectios" ,titleTextStyle:{ italic:false }},
                    hAxis: { title: "Percent Correct", titleTextStyle: { italic: false } },
                    colors: ['#a9c23f', '#d05a57', '#ddddde', '#f15a22'],
                    backgroundColor: { fill: '#ffffff' },
                    'chartArea': { 'width': '70%', 'height': '80%' },
                };
                var chart = new google.visualization.BarChart(document.getElementById("CommulativePerformance_chart"));
                chart.draw(view, options);
            }
        });

    }
    
}

// Peers comparion chart
function DrawPeerChart() {
    var examType = $('#ChartTypeId').val();
    var toggleValue = $('#toggle-switch1').is(":checked");
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    if (report.trim() == '' || report.trim() == "Select Date Range") {
        startDate = null;
        endDate = null;
    }
    else {
        var dateParts = report.split(' To ');
        startDate = dateParts[0].trim();
        endDate = dateParts[1].trim();
    }
    var jsonData = $.ajax({
        url: DashboardGoogleChart.url,
        type: 'POST',
        dataType: "json",
        data: { scoreCompWithPeers: toggleValue, examType: examType, fromDate: startDate, toDate: endDate },
        cache: false,
        success: function (result) {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'BcscSection');
            data.addColumn('number', 'Self');
            data.addColumn('number', 'Peer');
            data.addColumn({ name: 'Total', type: 'string', role: 'annotation' });
            $.each(result, function (key, value) {
                data.addRow([value.SubSpecialityName, value.SelfScore, value.PeerScore, '']);
            })
           
            var view = new google.visualization.DataView(data);
            view.setColumns([0,
                             1,
                             {
                                 calc: "stringify",
                                 sourceColumn: 1,
                                 type: "string",
                                 role: "annotation"
                             },
                             2,
                              {
                                  calc: "stringify",
                                  sourceColumn: 2,
                                  type: "string",
                                  role: "annotation"
                              },
            ]);
            var options = {
                width: '100%',
                height: 700,
                //legend: { position: 'top', maxLines: 3 },
                //tooltip: {isHtml: true},
                legend: 'none',
                bar: { groupWidth: '60%', groupHeight: '60%' },
                fontSize: 12,
                tooltip: false,
                //bar: { groupWidth: '90%', groupHeight: '90%' },
                isStacked: false,
                backgroundColor: 'transparent',
           //     vAxis: { title: "BCSC Sectios" ,titleTextStyle:{ italic:false }},
                hAxis: { title: "Percent Correct", titleTextStyle: { italic: false } },
                colors: ['#a9c23f', '#d05a57', '#ddddde', '#f15a22'],
                backgroundColor: { fill: '#ffffff' },
                'chartArea': { 'width': '70%', 'height': '80%' },
            };
            var chart = new google.visualization.BarChart(document.getElementById("CommulativePerformance_chart"));
            chart.draw(view, options);
        }
    });
}
var resizeTimeout;
$(window).resize(function () {
    clearTimeout(resizeTimeout);
    resizeTimeout = setTimeout(function () {
        DrawCumulativeChart();
    }, 500);
});