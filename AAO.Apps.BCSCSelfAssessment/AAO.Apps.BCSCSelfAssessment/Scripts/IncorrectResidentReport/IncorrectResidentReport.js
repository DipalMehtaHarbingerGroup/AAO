$.noConflict();
$(document).ready(function () {
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

    $("#filterBy").val(IncorrectDetails.QuestionCount).change();
    $("#QuestView").val(IncorrectDetails.QuestViewVal).change();

    if ($("#details").val() == undefined)
        $("#PageNavigation").hide();
    else
        pagination(IncorrectDetails.PageNoVal);

    var QuesViewVal = $('#QuestView option:selected').val();

    $("#displayCount").append("Displaying questions " + IncorrectDetails.firstQuestPerPage + " - " + IncorrectDetails.lastQuestPerPage + " of " + IncorrectDetails.RecordCount );
    // tabs / accordion
    $('.responsive-tabs').responsiveTabs({
        accordionOn: ['xs', 'sm']
    });
    $("#toggle_switch3").on("click", function () {

        var toogleVal = $("#toggle_switch3").is(":checked");

        if (toogleVal == true) {
            $("div[id$='collapseOne']").removeAttr("style");
            $("div[id$='collapseOne']").addClass("in");
        }
        if (toogleVal == false) {

            $("div[id$='collapseOne']").removeClass("in");
        }
    });
    $("#filterBy").on('change', function () {
        var PageNo = 1;
        reloadAssessment(PageNo);

    });

    $("#QuestView").on('change', function () {
        var PageNo = 1;

        reloadAssessment(PageNo);
    });
    function pagination(PageNoVal) {
        var totalnoofitem = IncorrectDetails.RecordCount;

        var limitperpage = $('#QuestView option:selected').val();;

        var totalpages = Math.round(totalnoofitem / limitperpage);

        var RoundVal = (totalnoofitem / limitperpage);

        RoundVal = RoundVal.toFixed(1);
        var output = RoundVal.split('.')[0];

        RoundVal = RoundVal - output;

        if (RoundVal > 0 && RoundVal < 0.5)
            RoundVal = 1
        else
            RoundVal = 0

        totalpages = totalpages + RoundVal;
        if (PageNoVal == undefined) {
            PageNoVal = 1;

        }
        for (var i = 1; i <= totalpages; i++) {
            if (i == PageNoVal)
                $(".pagination").append("<li  class='currentpage active'><a href='javascript:void(0)'>" + i + "</a></li>");
            else
                $(".pagination").append("<li class='currentpage'><a href='javascript:void(0)'>" + i + "</a></li>");
        }
        $(".pagination").append("<li id='Nextpage'> <a  href='javascript:void(0)'>Next</a></li>");

        $(".pagination li.currentpage").on("click", function () {
            if ($(this).hasClass("active")) {
                return false;
            }
            else {
                var currentpage = $(this).index();
                var getdata = reloadAssessment(currentpage);
                $(".pagination li").removeClass("active");
                $(this).addClass("active");
                $("#QuestionText").hide();
                var grandtotal = limitperpage * currentpage;
                for (var i = grandtotal - limitperpage; i < grandtotal; i++) {
                    $("#QuestionText:eq(" + i + ")").show();

                }

            }

        });
        $("#Nextpage").on("click", function () {
            var currentpage = $(".pagination li.active").index();

            if (currentpage === totalpages) {
                return false;
            } else {
                currentpage++;
                reloadAssessment(currentpage);

            }
        });
        $("#Previouspage").on("click", function () {
            var currentpage = $(".pagination li.active").index();

            if (currentpage === 1) {
                return false;
            } else {
                currentpage--;
                reloadAssessment(currentpage);
            }
        });
    }

    function reloadAssessment(PageNo) {
        if (PageNo == undefined)
            PageNo = $(".pagination li.active").index();
        var QuestionViewVal = $('#QuestView option:selected').val();
        var Filter = $("#filterBy option:selected").text();
        var SubSpecialityId = IncorrectDetails.SubSpecialityId;
        window.location.href = '/IncorrectResidentReport/IncorrectQuestionDetails' + '?SubspecialtyId=' + SubSpecialityId + '&ExamStartDate=' + null + '&ExamCompletedDate=' + null + '&NoOfRecords=' + QuestionViewVal + '&PageNo=' + PageNo;
    }


    $(".notes-panel, .panel-collapse").on("hide.bs.collapse", function () {
        var id = ($(this).attr("excerptid"));
        $(this).parent().find("#quesText" + id + " .question-number-view-exam-details_Excerpt i").removeClass("fa-angle-up").addClass("fa-angle-right");
    });
    $(".notes-panel, .panel-collapse").on("show.bs.collapse", function () {

        var id = ($(this).attr("excerptid"));
        $(this).parent().find("#quesText" + id + " .question-number-view-exam-details_Excerpt i").removeClass("fa-angle-right").addClass("fa-angle-up");
    });
});