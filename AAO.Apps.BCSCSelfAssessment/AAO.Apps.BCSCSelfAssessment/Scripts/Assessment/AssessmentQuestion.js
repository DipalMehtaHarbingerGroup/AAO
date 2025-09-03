
if (AssessmentQuestionViewModel.markedStatus == true) {
    $("#markQuestion").parent().find("i").removeClass('fa-star-o').addClass('fa-star');
}
if (AssessmentQuestionViewModel.markedStatus == false) {
    $("#markQuestion").parent().find("i").removeClass('fa-star').addClass('fa-star-o');
}

$(document).ready(function () {
    $("#VersionInfo").append(navigator.appVersion);
    var next = null;
    var prev = null;
    var examId = null;
    var currentQuestionId = null;
    var ExamattemptId = null;
    var isright = AssessmentQuestionViewModel.isRightId;
    var IsYourAnswer = null;
    IsYourAnswer = AssessmentQuestionViewModel.isYourAnswerCorrect;
    var next = AssessmentQuestionViewModel.next;
    var IsRightChoiceId = null;
    if (next == null || next == undefined) { next = -1; }
    prev = AssessmentQuestionViewModel.prev;
    if (prev == null || prev == undefined) { prev = -2; }
    examId = AssessmentQuestionViewModel.examId;
    currentQuestionId = AssessmentQuestionViewModel.currentQuestionId;
    examAttemptId = AssessmentQuestionViewModel.examAttemptId;
    var markedStatus = AssessmentQuestionViewModel.markedStatus;
    // Exam Mode
    var examMode = AssessmentQuestionViewModel.examMode;
    var examAnswerToShow = AssessmentQuestionViewModel.examAnswerToShow;
    //Exam Status
    var examStatus = null;
    examStatus = AssessmentQuestionViewModel.examStatus;
    //Image Enlarge
    /****************************************/
    $(".img11").off('click');
    $(".img11").on('click', function () {
        var img = $(this).attr('src'); // get image
        $('#showimg').empty();
        $('#showimg').attr('src', img); //load image in modal
    });
    /*****************************************************/
    $(".myDIV").off('click');
    $(".myDIV").on('click', function () {
        var imgSrc = $(this).parent().find("img").attr("src");
        $('#showimg').empty();
        $('#showimg').attr('src', imgSrc); //load image in modal

    })

    //$("#createNotes").draggable({
    //    handle: ".modal-header"
    //});

    $("#myModal").draggable({
        handle: ".modal-header"
    });

    // Added for HG-515 - In Challenge mode refresh issue on 25Aug2025
    if ($('#challengeModetext').val().trim().length > 0) {
        $('#answerChoice').show();
        /*if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() > 0) {
            $("#AnswerChoiceSelectDiv").show();
        }*/
    }


    // End of Image enlarge
    if (AssessmentQuestionViewModel.examMode == false) {
        $("#AnswerChoiceSelectDiv").hide();
        $('#ChallengeModediv').hide();
        $('#answerChoice').show();
    }
    else if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() == 0) {
        //$('#answerChoice').hide();
        $('#Contentcontainer').hide();
        $("#AnswerChoiceSelectDiv").hide();
        $("#ExplainationDiv").hide();
        $('#ChallengeModediv').show();
    }
    else if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() != 0) {
        $('#ChallengeModediv').show();
        $('#Contentcontainer').hide();
        $("#ExplainationDiv").hide();
        $("#AnswerChoiceSelectDiv").hide();
        $('#answerChoice').show();
    }
    else {
        $('#answerChoice').show();
        $('#Contentcontainer').show();

    }
    if (AssessmentQuestionViewModel.choiceId > 0)
    {
        $.ajax({
            type: 'POST',
            data: { questionId: AssessmentQuestionViewModel.currentQuestionId },
            url: '/Assessment/GetRightChoiceId',
            async: false,
            success: function (result) {
                AssessmentQuestionViewModel.rightChoiceId = result;
            }
        });
    }
    $('#challengeModetext').keyup(function () {
        $('#challengemodeSubmit').attr('disabled', !$('#challengeModetext').val());
    });

    $('#challengemodeSubmit').click(function () {
        challengemodeSubmitbutton();
    });

    function challengemodeSubmitbutton() {
         var examAttemptText = $("#challengeModetext").val();
        $.ajax({
            type: 'GET',
            data: { ExamId: AssessmentQuestionViewModel.examId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, ExamAttemptText: examAttemptText, TextType: 'C' },
            url: '/Assessment/AddNotesAnswerText',
            success: function (result) {
                $("#challengeModetext").attr('disabled', true);
                $("#challengemodeSubmit").attr('disabled', true);
                var examAttemptText = $("#challengeModetext").val();
                $('#answerChoice').show();
                $('#ChallengeModediv').show();
                $('#nextID').attr('disabled', true);
                $('#prevID').attr('disabled', true);
                if (!$("input[name='ChoiceAnswer']:checked").val()) {
                    $('#nextID').attr('disabled', true);
                    $('#prevID').attr('disabled', true);
                } else {
                    $('#nextID').attr('disabled', false);
                    $('#prevID').attr('disabled', false);
                }
            }
        });
        //$('#answerChoice').hide();
        $('#ChallengeModediv').hide();
        $("#AnswerChoiceSelectDiv").hide();
        $('#nextID').attr('disabled', true);
        $('#prevID').attr('disabled', true);
    }

    $('.DialogClose').on('click', function () {
        $("#NoteTextArea").val('');
        $(".Notespan").text('');
    });
    $("#CancelNoteBtn").on('click', function () {
        $("#NoteTextArea").val('');
        $(".Notespan").text('');
    });
    //Resume Exam Functionality  Code Check Show correct answer on/off

    if (AssessmentQuestionViewModel.examAnswerToShow == true)
    {
        $("#ExplainationDiv").hide();
        $('#Contentcontainer').hide();
        if ($("#userselectedchoiceId").val() > 0 && $("#isRightChoiceflag").val() == "False")
        {
            $("#ExplainationDiv").show();
            $('#Contentcontainer').show();
            $('#answertext' + $("#userselectedchoiceId").val()).html('');
            $('#radioBtn' + $("#userselectedchoiceId").val()).addClass('bg-danger flex-row-center');
            $('#answertext' + $("#userselectedchoiceId").val()).html('Your Answer');
            $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
            $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
            $('#challengeModetext').attr('disabled', true);
        }
        else if ($("#userselectedchoiceId").val() > 0 && $("#isRightChoiceflag").val() == "True")
        {
            $("#ExplainationDiv").show();
            $('#Contentcontainer').show();
            $('#answertext' + $("#userselectedchoiceId").val()).html('');
            $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
            $('#answertext' + $("#userselectedchoiceId").val()).html('Correct Answer');
            $('#challengeModetext').attr('disabled', true);
        }
        else if (AssessmentQuestionViewModel.rightChoiceId != "" && $("#userselectedchoiceId").val() > 0 && $("#isRightChoiceflag").val() != "")
        {
            $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('');
            $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
            $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
            $('#challengeModetext').attr('disabled', true);
        }
    }
    if (AssessmentQuestionViewModel.examAnswerToShow == false)
    {
        $("#ExplainationDiv").hide();
        $('#Contentcontainer').hide();
    }
    //User Selected choice
    $('.ChoiceAnswerclass').on('change', function () {
        OnSelectAnswer();
    });

    function OnSelectAnswer() {
        if (AssessmentQuestionViewModel.examType != 3) {
            GetRightChoiceIDAndUpateUI();
        }

        if (AssessmentQuestionViewModel.examType == 3) {
            EnableControl($("#SpacedRepetitionSubmitConfident"));
            EnableControl($("#SpacedRepetitionSubmitNotConfident"));
        }
    }

    //Added this function for SR Confident
    $("#SpacedRepetitionSubmitConfident").on("click", function (event) {
        SpacedRepetitionSubmitConfident(event, 3);
    });
    function SpacedRepetitionSubmitConfident(event, buttonType) {
        document.removeEventListener('keydown', hotKeyPressed);
        GetRightChoiceIDAndUpateUI(buttonType);
        SubmitUserAnswer(event, buttonType);
        ToggleNextButton(true);
        DisableControl($("#SpacedRepetitionSubmitNotConfident"));
        document.addEventListener('keydown', hotKeyPressed);
    }

    //Added this function for SR Not Confident
    $("#SpacedRepetitionSubmitNotConfident").on("click", function (event) {
        SpacedRepetitionSubmitNotConfident(event, 4);
    });
    function SpacedRepetitionSubmitNotConfident(event, buttonType) {
        document.removeEventListener('keydown', hotKeyPressed);
        GetRightChoiceIDAndUpateUI(buttonType);
        SubmitUserAnswer(event, buttonType);
        ToggleNextButton(true);
        DisableControl($("#SpacedRepetitionSubmitConfident"));
        document.addEventListener('keydown', hotKeyPressed);
    }

    function EnableControl(ctrl) {
        $(ctrl).removeAttr('disabled');
        $(ctrl).removeClass('disabled');
    }

    function DisableControl(ctrl) {
        $(ctrl).attr('disabled', true);
        $(ctrl).addClass('disabled');
    }

    function ToggleNextButton(isVisible)
    {
        if (isVisible == true) {
            $("#nextID").removeClass("hidden");
            $("#nextID").addClass("visible");
        }
        else {
            $("#nextID").removeClass("visible");
            $("#nextID").addClass("hidden");
        }
    }

    function GetRightChoiceIDAndUpateUI(ButtonType) {
        var choice = $("input[name='ChoiceAnswer']:checked").val();//$("input[type='radio'].ChoiceAnswerclass:checked").val();
        var examAttemptText = $("#challengeModetext").val();
        $.ajax({
            type: 'POST',
            data: { questionId: AssessmentQuestionViewModel.currentQuestionId },
            url: '/Assessment/GetRightChoiceId',
            async: false,
            success: function (result) {
                AssessmentQuestionViewModel.rightChoiceId = result;
            }
        });
        if (AssessmentQuestionViewModel.examAnswerToShow == true){
            AssessmentQuestionViewModel.rightChoiceId = AssessmentQuestionViewModel.rightChoiceId;
            if (AssessmentQuestionViewModel.examTimeType == 1) {
                timer.pause();
                timerpause = false;
            }
            if (AssessmentQuestionViewModel.rightChoiceId == $("input[name='ChoiceAnswer']:checked").val()) {
                //if (AssessmentQuestionViewModel.examTimeType == 1) {
                //    timer.pause();
                //    timerpause = false;
                //}
                $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
                $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
                $(".ChoiceAnswerclass").attr('disabled', true);
                isright = true;
                $('#userSelectedChoiceflag').val(isright);
                if (AssessmentQuestionViewModel.examMode == true) {
                    $('#nextID').attr('disabled', true);
                    $('#prevID').attr('disabled', true);
                }

                else {
                    $('#nextID').attr('disabled', false);
                    $('#prevID').attr('disabled', false);
                }
            } else {
                if (AssessmentQuestionViewModel.examMode == true) {
                    $('#radioBtn' + $("input[name='ChoiceAnswer']:checked").val()).addClass('bg-danger flex-row-center');
                    $('#answertext' + $("input[name='ChoiceAnswer']:checked").val()).html('Your Answer');
                    $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
                    $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
                    $(".ChoiceAnswerclass").attr('disabled', true);
                    isright = false;
                    $('#userSelectedChoiceflag').val(isright);
                }
                else {

                    $('#radioBtn' + $("input[name='ChoiceAnswer']:checked").val()).addClass('bg-danger flex-row-center');
                    $('#answertext' + $("input[name='ChoiceAnswer']:checked").val()).html('Your Answer');
                    $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
                    $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
                    if (AssessmentQuestionViewModel.examType == 1 || AssessmentQuestionViewModel.examAnswerToShow == false) {
                        $(".ChoiceAnswerclass").attr('disabled', false);
                    }
                    else {
                        $(".ChoiceAnswerclass").attr('disabled', true);
                    }

                    isright = false;
                    $('#userSelectedChoiceflag').val(isright);
                    $('#nextID').attr('disabled', false);
                    $('#prevID').attr('disabled', false);
                }
            }
        }
        else {
            if (AssessmentQuestionViewModel.rightChoiceId == $("input[name='ChoiceAnswer']:checked").val()) {
                //if (AssessmentQuestionViewModel.examTimeType == 1) {
                //    timer.pause();
                //    timerpause = false;
                //}
                if (AssessmentQuestionViewModel.examType == 1 || AssessmentQuestionViewModel.examAnswerToShow == false) {
                    $(".ChoiceAnswerclass").attr('disabled', false);
                }
                else {
                    $(".ChoiceAnswerclass").attr('disabled', true);
                }


                isright = true;
                $('#userSelectedChoiceflag').val(isright);
                // $('#nextID').attr('disabled',false);
                // $('#prevID').attr('disabled',false);
            }
            else {
                if (AssessmentQuestionViewModel.examType == 1 || AssessmentQuestionViewModel.examAnswerToShow == false) {
                    $(".ChoiceAnswerclass").attr('disabled', false);
                }
                else {
                    $(".ChoiceAnswerclass").attr('disabled', true);
                }
                isright = false;
                $('#userSelectedChoiceflag').val(isright);
                //$('#nextID').attr('disabled',false);
                //$('#prevID').attr('disabled',false);                
            }

            if (examMode == true && AssessmentQuestionViewModel.rightChoiceId == $("input[name='ChoiceAnswer']:checked").val()) {
                if (AssessmentQuestionViewModel.examAnswerToShow == false) {
                    $(".ChoiceAnswerclass").attr('disabled', false);
                }
                else {
                    $(".ChoiceAnswerclass").attr('disabled', true);
                }
                isright = true;
                $('#userSelectedChoiceflag').val(isright);
                $('#nextID').attr('disabled', true);
                $('#prevID').attr('disabled', true);

            }
        }
        var ChoiceradioValue = $("input[name='ChoiceAnswer']:checked").val();
        var array = $.map(AssessmentQuestionViewModel.choiceList, function (value, index) {
            return [value.choiceId];
        });
        var choiceIDList = array.join(", ");
        $.ajax({
            url: '/Assessment/ExamAttemptByuser',
            type: 'POST',
            cache: false,
            dataType: "html",
            data: { ExamId: AssessmentQuestionViewModel.examId, Questionid: AssessmentQuestionViewModel.currentQuestionId, ChoiceSelected: ChoiceradioValue, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, isRight: isright, userStatechoiceIDList: choiceIDList, ButtonType: ButtonType }
        }).done(function (result) {
            var resultData = JSON.parse(result);
            $("#peerdetailsOnload").hide();
            $("#peerdetailsOnSelect").show();
            PeerDetailsData(resultData);
            if (AssessmentQuestionViewModel.examMode == true) {
                $("#AnswerChoiceSelectDiv").show();
            }
            if (AssessmentQuestionViewModel.examAnswerToShow == true) {
                $("#ExplainationDiv").load();
                $("#ExplainationDiv").show();
                $('#Contentcontainer').show();
            }
            else {
                $("#ExplainationDiv").hide();
                $('#Contentcontainer').hide();
            }
        });
    }

    function PeerDetailsData(resultData) {
        $.each(resultData.PeerDetails, function (key, value) {
            var ChoiceValue = $("input[type='radio'].ChoiceAnswerclass:checked").val();

            var userComparion = parseInt(AssessmentQuestionViewModel.PeerUserType);
            if (value.IsRightChoice == true) {
                if (userComparion == 1) {
                    Mychoice = parseFloat(value.ChoicePercentage).toFixed() + "% of respondents answered correctly."
                } else {
                    Mychoice = parseFloat(value.ChoicePercentage).toFixed() + "% of peers answered correctly."
                }
                //var Mychoice = parseFloat(value.ChoicePercentage).toFixed() + "% of respondents answered correctly."
                $("#comparisonPercentSpan").append(Mychoice);
                var liTag = $("<li class=font-green></li>");
                var MyTag = $(liTag).append(value.ChoiceOption + ". " + parseFloat(value.ChoicePercentage).toFixed() + "%");
                $("#PeerDetailsDiv").append(MyTag);
            }
            else if ($("input[type='radio'].ChoiceAnswerclass:checked").val() == value.ChoiceId) {
                var liTag = $("<li class=font-red></li>");
                var MyTag = $(liTag).append(value.ChoiceOption + ". " + parseFloat(value.ChoicePercentage).toFixed() + "%");
                $("#PeerDetailsDiv").append(MyTag);
            }
            else {
                var liTag = $("<li></li>");
                var MyTag = $(liTag).append(value.ChoiceOption + ". " + parseFloat(value.ChoicePercentage).toFixed() + "%");
                $("#PeerDetailsDiv").append(MyTag);
            }
        });
    }
    $(".nextID").bind("click", function (event) {
        SubmitUserAnswer(event,2);
    });

    //Next button 
    function SubmitUserAnswer(event, buttonType) {
        document.removeEventListener('keydown', hotKeyPressed);
        timerpause = true;
        timer.start();
        var ChoiceradioValue = $("input[type='radio'].ChoiceAnswerclass:checked").val();
        var ExamAttempt = AssessmentQuestionViewModel.examAttemptId;
        event.preventDefault();
        $.ajax({
            url: '/Assessment/QuestionListIndex',
            type: 'POST',
            cache: false,
            dataType: "html",
            data: { ExamId: AssessmentQuestionViewModel.examId, Questionid: AssessmentQuestionViewModel.next, ChoiceSelected: ChoiceradioValue, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, isRight: isright, IsYourAnswer: IsYourAnswer, ButtonType: buttonType }
        }).done(function (result) {
            if (result != "") {
                $('#container').html("");
                $('#container').html(result);
                if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == true) {
                    $("#ExplainationDiv").show();
                    $('#Contentcontainer').show();
                }
                if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == false) {
                    $("#ExplainationDiv").hide();
                    $('#Contentcontainer').hide();
                }
                if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == true) {
                    $("#ExplainationDiv").show();
                    $('#Contentcontainer').show();
                    $("#inlineRadio1").prop("disabled", true);
                    $("#inlineRadio2").prop("disabled", true);
                    $("#AnswerChoiceSelectDiv").show();
                    if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
                        $("#inlineRadio1").prop("checked", true);
                    }
                    if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
                        $("#inlineRadio2").prop("checked", true);
                    }
                }
                if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == false) {
                    $("#ExplainationDiv").hide();
                    $('#Contentcontainer').hide();
                    $("#inlineRadio1").prop("disabled", true);
                    $("#inlineRadio2").prop("disabled", true);
                    $("#AnswerChoiceSelectDiv").show();
                    if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
                        $("#inlineRadio1").prop("checked", true);
                    }
                    if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
                        $("#inlineRadio2").prop("checked", true);
                    }
                }
            }
        });
    }
    $(".prevID").bind("click", function (event) {
        SubmitUserAnswerPrev(event, 1);
    });
    
    /* $("#prevID").bind("click", function () {*/
    //Next Prev
    function SubmitUserAnswerPrev(event, buttonType) {
        document.removeEventListener('keydown', hotKeyPressed);
        timerpause = true;
        timer.start();
        var ChoiceradioValue = $("input[type='radio'].ChoiceAnswerclass:checked").val();
        var markcheck1 = AssessmentQuestionViewModel.markedStatus;

        var ExamAttempt = AssessmentQuestionViewModel.examAttemptId;
        $.ajax({
            url: '/Assessment/QuestionListIndex',
            type: 'POST',
            cache: false,
            dataType: "html",
            data: { ExamId: AssessmentQuestionViewModel.examId, Questionid: AssessmentQuestionViewModel.prev, ChoiceSelected: ChoiceradioValue, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, isRight: isright, IsYourAnswer: IsYourAnswer, ButtonType: 1 }
        }).done(function (result) {

            $('#container').html("");
            $('#container').html(result);
            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == true) {
                $("#ExplainationDiv").show();
                $('#Contentcontainer').show();
            }
            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == false) {
                $("#ExplainationDiv").hide();
                $('#Contentcontainer').hide();
            }
            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examMode == true && AssessmentQuestionViewModel.examAnswerToShow == true) {
                $("#ExplainationDiv").show();
                $('#Contentcontainer').show();
                $("#inlineRadio1").prop("disabled", true);
                $("#inlineRadio2").prop("disabled", true);
                $("#AnswerChoiceSelectDiv").show();
                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
                    $("#inlineRadio1").prop("checked", true);
                }
                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
                    $("#inlineRadio2").prop("checked", true);
                }
            }
            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examMode == true && AssessmentQuestionViewModel.examAnswerToShow == false) {
                $("#ExplainationDiv").hide();
                $('#Contentcontainer').hide();
                $("#inlineRadio1").prop("disabled", true);
                $("#inlineRadio2").prop("disabled", true);
                $("#AnswerChoiceSelectDiv").show();
                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
                    $("#inlineRadio1").prop("checked", true);
                }
                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
                    $("#inlineRadio2").prop("checked", true);
                }
            }
        });
    }
    //Call for Submit
    $("#submit").bind("click", function () {
        $("#submit").attr('disabled', true);
        $.ajax({
            url: '/Assessment/SubmitExam',
            type: 'POST',
            cache: false,
            dataType: "html",
            data: { ExamId: AssessmentQuestionViewModel.examId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId },
            success: function (result) {
                if (result == 1) {
                    $('#SubmittedSuccessDialog').modal('show');
                    setTimeout(function () {
                        $('#SubmittedSuccessDialog').modal('hide');
                    }, 3000);

                    setTimeout(function () {
                        window.location.href = '/ExamHistory/Index';
                    }, 3000);

                }
                else {
                    $('#SubmittedErrorDialog').modal();

                }
            }
        });
    });
    /**submit end**/

    /**Add notes click*/
    $("#AddNoteBtn").on('click', function (event) {
        var trimNotesStr = $.trim($("#NoteTextArea").val());
        if (trimNotesStr == "") {
            var ErrNotesmsg = "Please enter your notes"
            $(".Notespan").text(ErrNotesmsg);
            return false;
        }
        else {
            var NotesTextAreaDetails = $("#NoteTextArea").val();

            $.ajax({
                type: 'post',
                data: { ExamId: AssessmentQuestionViewModel.examId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, ExamAttemptText: $("#NoteTextArea").val(), TextType: 'N' },
                url: '/Assessment/AddNotesAnswerText',
                success: function (result) {
                    $('#Contentcontainer').html("");
                    $('#Contentcontainer').html(result);
                    if (result.success) {

                    }
                }
            });
        }
        $("#NoteTextArea").val('');
    });

    $("#NoteTextArea").on('keypress', function () {
        $(".Notespan").text('');

    });
    /***Add notes click end****/

    $("#FeedbackTxt").on('keypress', function () {
        $(".FeedbackSpan").text('');
    })

    $("#view-progress ,#Next-view-progress").on('click', function () {
        var examAnswerToShow = AssessmentQuestionViewModel.examAnswerToShow;
        $('#table-container').empty();
        $('#SecondTableId').empty();
        $('#ThirdTableId').empty();
        $('#FourthTableId').empty();
        $("#header-search-div").css("display", "none");
        openNav();
        var containerData1 = $('#table-container');
        var containerData2 = $('#SecondTableId');
        var containerData3 = $('#ThirdTableId');
        var containerData4 = $('#FourthTableId');
        var tableHeader = "<thead><tr><td>Questions</td><td colspan='2'>Answers</td><tr></thead>"
        table1 = $('<table id="ViewProgTabId">' + tableHeader + '</table>');
        table2 = $('<table id="ViewProgTabId2">' + tableHeader + '</table>');
        table3 = $('<table id="ViewProgTabId3">' + tableHeader + '</table>');
        table4 = $('<table id="ViewProgTabId4">' + tableHeader + '</table>');
        $.ajax({
            url: '/Assessment/ViewProgress',
            type: 'POST',
            cache: false,
            dataType: "html",
            data: { ExamId: AssessmentQuestionViewModel.examId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId },
            success: function (result) {
                var ViewProgressData = JSON.parse(result);
                if ($(window).width() >= 769) {

                    var FIRST = 0, SECOND = 0, THIRD = 0, FOURTH = 0;
                    for (var z = 1; z <= ViewProgressData.length; z++) {
                        if ((z % 4) == 1)
                            FIRST++;
                        else if ((z % 4) == 2)
                            SECOND++;
                        else if ((z % 4) == 3)
                            THIRD++;
                        else if ((z % 4) == 0)
                            FOURTH++;
                    }
                    var TotalRecords = Math.floor(ViewProgressData.length / 4);

                    var i = 0;
                    $.each(ViewProgressData, function (key, value) {
                        var tr = $('<tr>');

                        tr.append('<td style="cursor: pointer;" id="QustionRedirect" ExamId="' + AssessmentQuestionViewModel.examId + '" QuestionId="' + value.QuestionId + '"  ExamattemptId="' + AssessmentQuestionViewModel.examAttemptId + '" ><a>' + value.QuestionSequence + '</a></td>');
                        if (value.selectedOption != value.correctOption && value.selectedOption != 'u' && AssessmentQuestionViewModel.examAnswerToShow == true) {
                            tr.append('<td><span id="unique' + value.QuestionId + '" class="striked-font font-red">' + value.SelectedOption + '</span></td>');
                        }
                        else {
                            var SelectedOptions = "";
                            if (value.SelectedOption == 'u') {
                                SelectedOptions = "[u]";
                            } else {
                                SelectedOptions = value.SelectedOption;
                            }
                            tr.append('<td><span id="unique' + value.QuestionId + '">' + SelectedOptions + '</span></td>');
                        }
                        i++;
                        if (i <= FIRST) {
                            table1.append(tr);
                        }
                        else if (i <= SECOND + FIRST) {
                            table2.append(tr);
                        } else if (i <= THIRD + SECOND + FIRST) {
                            table3.append(tr);
                        } else if (i <= FOURTH + THIRD + SECOND + FIRST) {
                            table4.append(tr);
                        }
                        var d = new Date();
                        var n = d.getTime();
                        var correctoption1 = "correctoption" + n;
                        var CorrectChoiceData = "", AddMarkSymbol = "", Addnotessymbol = "";
                        var correctOptonId = Date.now();
                        if (value.SelectedOption != 'u' && AssessmentQuestionViewModel.examAnswerToShow == true && value.SelectedOption != value.CorrectOption) {
                            CorrectChoiceData = '<td id="' + correctoption1 + '"><span class="correctoptionspan">' + value.CorrectOption + '</span>';
                        }
                        else {
                            CorrectChoiceData = '<td id="' + correctoption1 + '">';
                        }
                        if (value.IsMarked == true) {

                            AddMarkSymbol = '&nbsp;&nbsp;<i class="fa fa-star-o text-center"></i>'

                        }
                        if (value.IsMarked == false) {

                            AddMarkSymbol = '';

                        }

                        if (value.HasNotes == true) {
                            Addnotessymbol = '&nbsp;<i class="fa fa-sticky-note-o text-center"></i>'
                        }
                        else {
                            Addnotessymbol = ''
                        }

                        tr.append(CorrectChoiceData + AddMarkSymbol + Addnotessymbol + '</td>');
                    });
                    containerData1.append(table1);
                    containerData2.append(table2);
                    containerData3.append(table3);
                    containerData4.append(table4);
                }
                else {
                    var TotalRecords = Math.floor(ViewProgressData.length);
                    var i = 0;
                    $.each(ViewProgressData, function (key, value) {

                        var tr = $('<tr>');
                        tr.append('<td style="cursor: pointer;" id="QustionRedirect" ExamId="' + AssessmentQuestionViewModel.examId + '" QuestionId="' + value.QuestionId + '"  ExamattemptId="' + AssessmentQuestionViewModel.examAttemptId + '" ><a>' + value.QuestionSequence + '</a></td>');
                        if (value.SelectedOption != value.CorrectOption && value.SelectedOption != 'u' && AssessmentQuestionViewModel.examAnswerToShow == true) {
                            tr.append('<td><span id="unique' + value.QuestionId + '" class="striked-font font-red">' + value.SelectedOption + '</span></td>');
                        }
                        else {
                            var SelectedOptions = "";
                            if (value.SelectedOption == 'u') {
                                SelectedOptions = "[u]";
                            } else {
                                SelectedOptions = value.SelectedOption;
                            }
                            tr.append('<td><span id="unique' + value.QuestionId + '">' + SelectedOptions + '</span></td>');
                        }

                        table1.append(tr);


                        var d = new Date();
                        var n = d.getTime();
                        var correctoption1 = "correctoption" + n;
                        var CorrectChoiceData = "", AddMarkSymbol = "", Addnotessymbol = "";
                        var correctOptonId = Date.now();
                        if (value.SelectedOption != 'u' && AssessmentQuestionViewModel.examAnswerToShow == true && value.SelectedOption != value.CorrectOption) {
                            CorrectChoiceData = '<td id="' + correctoption1 + '"><span class="correctoptionspan">' + value.CorrectOption + '</span>';
                        }
                        else {
                            CorrectChoiceData = '<td id="' + correctoption1 + '">';
                        }
                        if (value.IsMarked == true) {
                            AddMarkSymbol = '&nbsp;&nbsp;<i class="fa fa-star-o text-center"></i>'

                        }
                        if (value.IsMarked == false) {
                            AddMarkSymbol = '';

                        }

                        if (value.HasNotes == true) {
                            Addnotessymbol = '&nbsp;<i class="fa fa-sticky-note-o text-center"></i>'
                        }
                        else {
                            Addnotessymbol = ''
                        }

                        tr.append(CorrectChoiceData + AddMarkSymbol + Addnotessymbol + '</td>');
                    });

                    containerData1.append(table1);




                }

                $("#ViewProgTabId").addClass('table table-bordered table-striped');
                $("#ViewProgTabId2").addClass('table table-bordered table-striped');
                $("#ViewProgTabId3").addClass('table table-bordered table-striped');
                $("#ViewProgTabId4").addClass('table table-bordered table-striped');
            }
        });

        $(".closebtn").off('çlick');
        $(".closebtn").on('click', function () {
            closeNav();
            $("#header-search-div").css("display", "block");
        })
        function openNav() {
            $("#myNav").show();
            $('body').addClass('my-body-noscroll-class');
        }
        function closeNav() {
            $("#myNav").hide();
            $('body').removeClass('my-body-noscroll-class');
        }
    });
    // End of View Progress

    //$("#saveAndExit").bind("click", function () {
    $(".saveAndExit").bind("click", function () {
        var ChoiceradioValue = $("input[name='ChoiceAnswer']:checked").val();
        var isTrue = true;
        var ExamAttempt = parseInt($("#AssExamattemptId").val())
        window.location.href = '/Assessment/QuestionListIndex' + '?ExamId=' + AssessmentQuestionViewModel.examId + '&Questionid=' + AssessmentQuestionViewModel.prev + '&ChoiceSelected=' + ChoiceradioValue + '&ExamattemptId=' + ExamAttempt + '&CurrentQuestionid=' + AssessmentQuestionViewModel.currentQuestionId + '&isRight=' + isright + '&isExit=' + isTrue;
    });
    $(document).on('click', "#QustionRedirect", function () {
        var examId = $(this).attr("examId");
        var questionId = $(this).attr("questionId");
        window.location.href = '/Assessment/Index' + '?ExamId=' + AssessmentQuestionViewModel.examId + '&Questionid=' + questionId + '&ExamattemptId=' + AssessmentQuestionViewModel.examAttemptId;
    });

    //Mark QUestion

    $("#markQuestion, #Markstatus").click(function () {
        var val = ($("#markQuestion").attr("alreadyMarked").toLowerCase() === "true");
        $.ajax({
            url: '/Assessment/MarkQuestionCheck',
            type: 'POST',
            data: { ExamId: AssessmentQuestionViewModel.examId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, MarkedStatus: val },
            success: function (result) {
                if (result == true) {
                    $("#markQuestion").html("Bookmarked").addClass("hidden-xs");
                    $("#markQuestion").attr("alreadyMarked", true);
                    $("#markQuestion").parent().find("i").removeClass('fa-star-o').addClass('fa-star');
                }
                else if (result == false) {

                    $("#markQuestion").html("Bookmark").addClass("hidden-xs");
                    $("#markQuestion").attr("alreadyMarked", false);
                    $("#markQuestion").parent().find("i").removeClass('fa-star').addClass('fa-star-o');

                }
            }
        });
    });

    //is your answer
    $(".AnswerRadio").on("change", function () {
        var YourAnswer = $("input[name='inlineRadioOptions']:checked").val();
        if (YourAnswer == "yes") {
            IsYourAnswerSelected = 1;


        } else if (YourAnswer == "no") {
            IsYourAnswerSelected = 2;
        }
        $.ajax({
            type: 'post',
            data: { ExamId: AssessmentQuestionViewModel.examId, Questionid: AssessmentQuestionViewModel.currentQuestionId, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, IsYourAnswer: IsYourAnswerSelected },
            url: '/Assessment/SaveIsYourAnswer',
            success: function (result) {
                $("#prevID").attr('disabled', false);
                $("#nextID").attr('disabled', false);
            }
        });
    }); 
    (function () {
        var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
        if (iOS) {
            $('body').addClass('iOS-device');
        }
        var scrollPosition;

        $('.modal').on('show.bs.modal', function () {
            scrollPosition = $(window).scrollTop();
            $('.iOS-device').css('top', -scrollPosition);
        });

        $('.modal').on('hide.bs.modal', function () {
            $('.iOS-device').removeClass('modal-open');
            $('.iOS-device').css('top', 0);
            $(document).scrollTop(scrollPosition);
        });

    })();

    function nospaces(t) {
        if (t.value.match(/^\s+/g, "")) {
            t.value = t.value.replace(/\s/g, '');
        }
    }
    var element = '<div id="myForm"><span class="FeedbackSpan error-text-block"></span><form id="popForm"><textarea rows="3" placeholder="Send the Academy a comment about this question or features of the application." name="FeedbackTxt" id="FeedbackTxt" style="width:300px;height:180px;"></textarea><button type="button" class="btn btn-primary pull-right martop10 marbot10" id="sendFeedbackBtn" data-loading-text="Sending info.." data-target="#mySendFeedbackModal"><em class="icon-ok"></em> Send</button><button type="button" class="btn btn-default pull-right martop10 marbot10 mar10" id="cancelFeedbackBtn" data-loading-text="Sending info.."><em class="icon-ok"></em> Cancel</button></form></div>'
    $('#Feedback').popover({
        placement: 'bottom',
        title: 'Share feedback',
        html: true,
        content: element
    }).on('click', function () {

        var rightChoiceId = AssessmentQuestionViewModel.rightChoiceId;
        var myArray = [], myArray1 = [];
        $.each(AssessmentQuestionViewModel.choiceList, function (key, value) {
            myArray.push(value.choiceText);
            myArray1.push(value.choiceId);
        })
        for (var i = 0; i < myArray.length; i++) {
            myArray1[i];
            myArray[i];
            if (rightChoiceId == myArray1[i]) {
                myArray[i] = '<b>' + myArray[i] + '</b>';
            }

        }
        var browserPlatform = $("#VersionInfo").text();
        var currentQuestionId = AssessmentQuestionViewModel.friendlyId;
        var questionText = $("#labelquestionTextId").text();
        var discussion = $("#discussion").text();
        var references = $("#references").text();
        var userMailId = $("#userEmail").text();
        var userName = $("#userName").text();
        var QuestionTextArea = "Question :" + $("#labelquestionTextId").text();

        $("textarea#FeedbackTxt").val();
        $('#sendFeedbackBtn.btn-primary').click(function () {
            $('#Feedback').popover('hide');
            $body = $("body");
            $body.addClass("loading");
            setTimeout(function () { }, 5000);
            if ($("#FeedbackTxt").val() == 0) {
                var FeedbackError = " Please enter your feedback, which will be sent by email to the Academy."
                $(".FeedbackSpan").text(FeedbackError);
                return false;
            }
            else {
                var strFileName = "";
                var feedbackTextArea = "User Name: " + userName + "<br />" +
                    "User Email: " + userMailId + "<br /><br />" +
                    "Browser and Platform: " + browserPlatform + "<br /><br />" +
                    "Feedback: " + $("textarea#FeedbackTxt").val() + "<br /><br />"
                    + "Question ID: " + currentQuestionId + "<br /><br />" +
                    "Topic: " + AssessmentQuestionViewModel.topicName + "<br /><br />" +
                    "Question: " + questionText + "<br /><br />" +
                    "1." + myArray[0] + "<br />" + "2." + myArray[1] + "<br />" + "3." + myArray[2] + "<br />" + "4." + myArray[3] + "<br /><br />" +
                    "Discussion: " + discussion + "<br /><br />" +
                    "References: " + references;
                feedbackTextArea = encodeURIComponent(feedbackTextArea);
                $.ajax({
                    type: 'POST',
                    data: { feedbackBody: feedbackTextArea, id: currentQuestionId, strFileName: strFileName, userName: userName, emailId: null },
                    url: '/Assessment/SendFeedback',
                    dataType: "html",
                    success: function (result) {
                        if (result == 1) {
                            $body.removeClass("loading");
                            $('#Feedback').popover('hide');
                            $("#myForm").html('');
                            $("#sendFeedbackconfirmDialog").modal();
                        }
                        else {
                            $body.removeClass("loading");
                            $("#sendFeedbackErrorDialog").modal();
                        }
                    }
                });
            }
        })
        $("#cancelFeedbackBtn").on('click', function () {
            $('#Feedback').popover('hide');
            $("#myForm").html('');

        })
    });
    // $('a[href^="http://"], a[href^="https://"]').not('a[href*=gusdecool]').attr('target', '_blank');

    $(".notes-panel, .panel-collapse").on("hide.bs.collapse", function () {
        var id = ($(this).attr("excerptid"));
        $(this).parent().find("#quesText" + id + " .question-number-view-exam-details_Excerpt i").removeClass("fa-angle-up").addClass("fa-angle-right");
    });
    $(".notes-panel, .panel-collapse").on("show.bs.collapse", function () {

        var id = ($(this).attr("excerptid"));
        $(this).parent().find("#quesText" + id + " .question-number-view-exam-details_Excerpt i").removeClass("fa-angle-right").addClass("fa-angle-up");
    });

 //Hot key pressed implementation for Option radio button, NextPrev button selection started form here

    //After refresh this page Add Event(add hotkeypressed function) which removed from Next & Prev calls 
    document.addEventListener('keydown', hotKeyPressed);
    
    function hotKeyPressed() {
        
        //checking if focus create note OR feedback OR search OR challengeMode text is there then return(do nothing)
        if (($('#createNotes').is(':visible')) || ($('#myForm').length > 0) || ($("#headersearch").is(":focus")) || ($('#challengeModetext').is(":focus")))
        {
            return;
        }

        //checking which hotkey pressed 
        if (event.type === 'keydown' && [65, 66, 67, 68, 78, 110, 80, 112].indexOf(event.keyCode) !== -1) {
                     
            //For Next Button
            if ( !($("#nextID").is(':disabled')) || !($("#prevID").is(':disabled')) ) {
                if (event.keyCode == 78 || event.keyCode == 110) {
                    if ($('#Next-view-progress').length > 0) {
                        $("#Next-view-progress").click();
                    }
                    else if ($('#nextID').is(':visible')){
                        SubmitUserAnswer(event, 2);
                    }
                    return;
                }

                //For Prev Button
                if (event.keyCode == 80 || event.keyCode == 112) {
                    if ($('#prevID').length > 0) {
                        SubmitUserAnswerPrev(event, 1);
                    }
                    else {
                        return;
                    }
                }
            }

            //If Ans is already disabled(Already answered) then do return(do nothing)
            if ($("input[name=ChoiceAnswer]").is(':disabled'))
            {   
                return;
            }

            //for selection of Option(a,b,c,d)
            if ($("#answerChoice").is(':visible')) {           
                switch (event.keyCode) {
                    case 65:
                        {
                             $('input:radio[name=ChoiceAnswer]')[0].checked = true;
                             break;           
                        }
                    case 66:
                        {
                            $('input:radio[name=ChoiceAnswer]')[1].checked = true;
                            break;
                        }
                    case 67:
                        {
                            $('input:radio[name=ChoiceAnswer]')[2].checked = true;
                            break;
                        }
                    case 68:
                        {
                            $('input:radio[name=ChoiceAnswer]')[3].checked = true;
                            break;
                        }
                }
            }

            //For Submit ans in Exam(trigger) in Show Ans type Exam 
            $('input:radio[name=ChoiceAnswer]:checked').trigger('change');
            
        }
    }
});
