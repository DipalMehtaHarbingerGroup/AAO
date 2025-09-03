$(function () {   
    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        FilterByDate();
    });

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

    FilterByDate();
    //var startDate;
    //var endDate;
    //var report = $('#reportrange').text();
    //if (report.trim() == '') {
    //    startDate = null;
    //    endDate = null;

    //}
    //else {
    //    var dateParts = report.split(' To ');
    //    startDate = dateParts[0];
    //    endDate = dateParts[1];
    //}
    //   $('#ResidentReport').DataTable({
    //        "processing": true,
    //        "serverSide": true,
    //        "filter": false,
    //        "orderMulti": false,
    //        "paging": false,
    //        "responsive": true,
    //        "ajax": {
    //           // "data": { "examStartDate": startDate, "examCompletedDate": endDate },
    //            "type": "POST",
    //            "url": '/AggregatePerformance/ReportDetails' + '?examStartDate=' + startDate + '&examCompletedDate=' + endDate,
    //        },
    //        "language": {
    //            "lengthMenu": "Show _MENU_ exams",
    //            "sEmptyTable": "No Records found",
    //            "info": ""

    //        },
    //        "columns": [
    //             //{ "data": "BCSCSectionNumber", "bSortable": false },
    //             {
    //                 data: null, "bSortable": false, render: function (data, type, row) {
    //                     return '<a href="/ResidentChapterReport/ResidentChapterReportDetails?subspecialtyId=' + data.SubspecialtyId + '&examStartDate=' + data.ExamStartDate + '&examCompletedDate=' + data.ExamCompletedDate + '" class="editAsset">' + 'Section ' + data.BCSCSectionNumber + ': ' + data.SubspecialtyName + '</a>';
    //                 }
    //             },
    //             { "data": "Correct", "bSortable": false },
    //             {
    //                 data: null, "bSortable": false, render: function (data, type, row) {
    //                     if (data.InCorrect == 0) {
    //                         return 0;
    //                     }
    //                     else {

    //                         return '<a href="/IncorrectRessidentReport/IncorrectQuestionDetails?subspecialtyId=' + data.SubspecialtyId + '&examStartDate=' + data.ExamStartDate + '&examCompletedDate=' + data.ExamCompletedDate + '" class="editAsset">' + data.InCorrect + '</a>';
    //                     }
    //                 }
    //             },
    //             {
    //                 "data": null, "bSortable": false, render: function (data, type, row) {

    //                     return data.Score + '%';
    //                 }
    //             },

    //        ],


    //   });
    
    });
       
function FilterByDate() {
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
    $('#ResidentReport').dataTable().fnDestroy();
    var ResidentReport = $('#ResidentReport').dataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "paging": false,
        "responsive": true,
        "ajax": {
            "type": "POST",
            "url": '/AggregatePerformance/ReportDetails' + '?examStartDate=' + startDate + '&examCompletedDate=' + endDate,
        },
       
        "language": {
            "lengthMenu": "Show _MENU_ exams",
            "sEmptyTable": "No Records found",
            "info": ""

        },
        "columns": [
          
             {
                data: null, "bSortable": false, render: function (data, type, row) {
                     return '<a href="/ResidentChapterReport/ResidentChapterReportDetails?subspecialtyId=' + data.SubspecialtyId + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate + '&SectionNumber=' + data.BCSCSectionNumber + '&SectionTitle=' + data.SubspecialtyName + '" class="editAsset">' + 'Section ' + data.BCSCSectionNumber + ': ' + data.SubspecialtyName + '</a>';

                    }
             },
               {
                   "data": null, "bSortable": false, render: function (data, type, row) {
                       return data.Score + '%';
                   }
               },
           
             {
                 data: null, "bSortable": false, render: function (data, type, row) {
                     if (data.InCorrect == 0) {
                         return 0;
                     }
                     else {

                         return '<a href="/IncorrectResidentReport/IncorrectQuestionDetails?subspecialtyId=' + data.SubspecialtyId + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate + '" class="editAsset">' + data.InCorrect + '</a>';
                     }
                 }
             },
             { "data": "Correct", "bSortable": false },
        ],
    });

}
$("#Export").on('click', function (event) {
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
    window.location.href = '/AggregatePerformance/ExportToExcel' + '?examStartDate=' + startDate + '&examCompletedDate=' + endDate;

});






