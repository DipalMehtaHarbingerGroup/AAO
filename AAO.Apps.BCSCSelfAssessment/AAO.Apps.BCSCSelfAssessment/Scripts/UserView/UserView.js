$(document).ready(function () {
    $("#VersionInfo").append(navigator.appVersion);
    var next = null;
    var prev = null;
    var isright = AssessmentQuestionViewModel.isRightId;
    var IsYourAnswer = null;
    IsYourAnswer = AssessmentQuestionViewModel.isYourAnswerCorrect;
    var next = AssessmentQuestionViewModel.next;
    if (next == null || next == undefined) { next = -1; }
    prev = AssessmentQuestionViewModel.prev;
    if (prev == null || prev == undefined) { prev = -2; }
    examId = AssessmentQuestionViewModel.examId;
    currentQuestionId = AssessmentQuestionViewModel.currentQuestionId;
    examAttemptId = AssessmentQuestionViewModel.examAttemptId;
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

    $("#myModal").draggable({
        handle: ".modal-header"
    });


    // End of Image enlarge
    if (AssessmentQuestionViewModel.examMode == false) {
        $("#AnswerChoiceSelectDiv").hide();
        $('#ChallengeModediv').hide();
        $('#answerChoice').show();
    }
    else {
        $('#answerChoice').show();
        $('#Contentcontainer').show();

    }
  
    //User Selected choice
    $('.ChoiceAnswerclass').on('change', function () {        
            GetRightChoiceIDAndUpateUI();
    });

    function EnableControl(ctrl) {
        $(ctrl).removeAttr('disabled');
        $(ctrl).removeClass('disabled');
    }

    function DisableControl(ctrl) {
        $(ctrl).attr('disabled', true);
        $(ctrl).addClass('disabled');
    }

    function ToggleNextButton(isVisible) {
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
        var choice = $("input[type='radio'].ChoiceAnswerclass:checked").val();
        if (AssessmentQuestionViewModel.examAnswerToShow == true) {
            AssessmentQuestionViewModel.rightChoiceId = AssessmentQuestionViewModel.rightChoiceId;
            
            if (AssessmentQuestionViewModel.rightChoiceId == $("input[name='ChoiceAnswer']:checked").val()) {
                $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
                $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
                $(".ChoiceAnswerclass").attr('disabled', true);
                isright = true;
                $('#userSelectedChoiceflag').val(isright);
                $("#ExplainationDiv").load();
                $("#ExplainationDiv").show();
                $('#Contentcontainer').show();
               
            }else {

                    $('#radioBtn' + $("input[name='ChoiceAnswer']:checked").val()).addClass('bg-danger flex-row-center');
                    $('#answertext' + $("input[name='ChoiceAnswer']:checked").val()).html('Your Answer');
                    $('#radioBtn' + AssessmentQuestionViewModel.rightChoiceId).addClass('bg-success flex-row-center');
                    $('#answertext' + AssessmentQuestionViewModel.rightChoiceId).html('Correct Answer');
                    $(".ChoiceAnswerclass").attr('disabled', true);

                $("#ExplainationDiv").load();
                $("#ExplainationDiv").show();
                $('#Contentcontainer').show();
                   
            }
        }
    }

    $("#resetQuestions").on('click', function () {
        window.location.href = '/UserView/Index';
    });

    $("#searchOnIDTitle").on('click', function () {
            SearchByID();
    });

    function SearchByID() {
        var QuestionId = $("#search").val();
        if ($.isNumeric(QuestionId)) {
            window.location.href = '/UserView/GetQuestionByFriendly' + '?QuestionId=' + QuestionId;
        }
        else {
            alert("Please enter valid question ID");
            $("#search").val('');
            $('#questionId').removeAttr('value');
        }

    }


    //$("#nextID").bind("click", function (event) {
    //    SubmitUserAnswer(event, 2);
    //});

    //function SubmitUserAnswer(event, buttonType) {
    //    timerpause = true;
    //    timer.start();
    //    var ChoiceradioValue = $("input[type='radio'].ChoiceAnswerclass:checked").val();
    //    var ExamAttempt = AssessmentQuestionViewModel.examAttemptId;
    //    event.preventDefault();
    //    $.ajax({
    //        url: '/Assessment/QuestionListIndex',
    //        type: 'POST',
    //        cache: false,
    //        dataType: "html",
    //        data: { ExamId: AssessmentQuestionViewModel.examId, Questionid: AssessmentQuestionViewModel.next, ChoiceSelected: ChoiceradioValue, ExamattemptId: AssessmentQuestionViewModel.examAttemptId, CurrentQuestionid: AssessmentQuestionViewModel.currentQuestionId, isRight: isright, IsYourAnswer: IsYourAnswer, ButtonType: buttonType }
    //    }).done(function (result) {
    //        if (result != "") {
    //            $('#container').html("");
    //            $('#container').html(result);
    //            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == true) {
    //                $("#ExplainationDiv").show();
    //                $('#Contentcontainer').show();
    //            }
    //            if ($("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == false) {
    //                $("#ExplainationDiv").hide();
    //                $('#Contentcontainer').hide();
    //            }
    //            if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == true) {
    //                $("#ExplainationDiv").show();
    //                $('#Contentcontainer').show();
    //                $("#inlineRadio1").prop("disabled", true);
    //                $("#inlineRadio2").prop("disabled", true);
    //                $("#AnswerChoiceSelectDiv").show();
    //                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
    //                    $("#inlineRadio1").prop("checked", true);
    //                }
    //                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
    //                    $("#inlineRadio2").prop("checked", true);
    //                }
    //            }
    //            if (AssessmentQuestionViewModel.examMode == true && $("#userselectedchoiceId").val() > 0 && AssessmentQuestionViewModel.examAnswerToShow == false) {
    //                $("#ExplainationDiv").hide();
    //                $('#Contentcontainer').hide();
    //                $("#inlineRadio1").prop("disabled", true);
    //                $("#inlineRadio2").prop("disabled", true);
    //                $("#AnswerChoiceSelectDiv").show();
    //                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 1) {
    //                    $("#inlineRadio1").prop("checked", true);
    //                }
    //                if (AssessmentQuestionViewModel.isYourAnswerCorrect == 2) {
    //                    $("#inlineRadio2").prop("checked", true);
    //                }
    //            }
    //        }
    //    });
    //}

    $("#prevID").bind("click", function () {

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
});
