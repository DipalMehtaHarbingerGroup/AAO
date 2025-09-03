(function () {
    var CustomExamType = null;
    var SumulatedExamType = null;
    var CustomIDArray = [];
    var totalQuestionArray = [];
    var markquestionArray = [];
    var incorrectasnwerArray = [];
    var examskipquestionArray = [];
    var QuickType = null;
    $('#NoofQuestions').prop('type', 'text');
  
    $.validator.unobtrusive.adapters.addBool("TypeofCategoryList", "required");
 
     $('#ExamMode').off('change');
    $('#ExamMode').on('change', function () {      
        if ($(this).prop('checked') == true) {
            $('#ExamTimeType').prop('checked', false);
            $('#ExamAnswerToShow').prop('checked', true);
            $('#ExamAnswerToShow').attr('disabled', 'disabled');
            $("#ExamTimeType").attr('disabled', true);
        }
        else {
            $('#ExamAnswerToShow').prop('checked', false);
            $('#ExamAnswerToShow').removeAttr('disabled');
            $('#ExamTimeType').removeAttr('disabled');
            
        }      
    });
    var Total = $("#spanValue_0").text();
    var Skip = $("#spanValue_1").text();
    var Incorrect = $("#spanValue_2").text();
    var Marked = $("#spanValue_3").text();
    var Total, Skip, Incorrect, Marked;
    $("#spanValue_0").text(0);
    $("#spanValue_1").text(0);
    $("#spanValue_2").text(0);
    $("#spanValue_3").text(0);
   
    $("input[type='hidden'][name='ExamMode']").remove();
    $("input[type='hidden'][name='ExamAnswerToShow']").remove();
    $("input[type='hidden'][name='ExamTimeType']").remove();

    //Get ExamType Count
    var custom_Mode_Count = 0;
    var simulated_Mode_Count = 0;
    var quick_Mode_Count = 0;
    var SpacedRepetition_Mode_Count = 0;
    $.ajax({
        type: 'POST',
        url: '/ExamManager/GetExamCountOnExamType',
        success: function (result) {
            custom_Mode_Count = result.CustomModeCount;
            simulated_Mode_Count = result.SimulatedModeCount;
            quick_Mode_Count = result.QuickModeCount;
            SpacedRepetition_Mode_Count = result.SRCount;
            $('#ExamName').val("Exam_custom_mode_" + custom_Mode_Count);
 
            if ($("#ExamType").val() == 2) {
                $('#ExamName').val("Exam_quick_mode_" + quick_Mode_Count);
            }
            if ($("#ExamType").val() == 3) {
                $('#ExamName').val("Exam_quick_mode_" + SpacedRepetition_Mode_Count);
            }
        }
    });
  
   
        $('#ExamType').on('change', function () {
        if (parseInt(this.value) == 1) {
            $('#custommode').show();
            $('#anothermode').hide();
            $('#custom').hide();
            $('#quick').hide();
            $("#NoofQuestions").val('');
            $('#selectall').attr('checked', false);
            $('.TypeofCategoryList').attr('checked', false);
            $('#ExamModeDiv').hide();
            $('#ExamName').val("Exam_Simulated_mode_" + simulated_Mode_Count);
            var SimulatesMsg = "Simulated exams have 260 questions and a 5-hour time limit.";
            $(".SimulatedTextSpan").text(SimulatesMsg);
            $(".ExamTitleSpan").text('');
            $(".NoOfQuesSpan").text('');
            $("#ErrorMsg").text('');
            $("#QuesTypeErr").text("");
            $("#spanValue_0").text(0);
            $("#spanValue_1").text(0);
            $("#spanValue_2").text(0);
            $("#spanValue_3").text(0);
            $('#ExamAnswerToShow').val('False');
            $('#ExamMode').val('False');

        }
        else if (parseInt(this.value) == 2) {
            $('#custommode').show();
            $('#anothermode').show();
            $('#custom').show();
            $('#quick').hide();
            $("#NoofQuestions").val(15);
            $('#ExamModeDiv').hide();
            $(".SimulatedTextSpan").text("");
            $('.TypeofCategoryList').attr('checked', false);
            $('#selectall').attr('checked', false);
           // $("#NoofQuestions").attr('disabled', true);
            $("#ExamTypeTimeDiv").hide();
            $('#ExamName').val("Exam_quick_mode_" + quick_Mode_Count);
            $('#questionfilter_0').attr('checked', false);
            $('#questionfilter_1').attr('checked', false);
            $('#questionfilter_2').attr('checked', false);
            $('#questionfilter_3').attr('checked', false); // Unchecks it
            //remove attribute disabled
            $('#questionfilter_1').removeAttr('disabled');
            $("#questionfilter_2").removeAttr('disabled');
            $("#questionfilter_3").removeAttr('disabled');
            $("#questionfilter_0").prop("checked", true);
            ///////////////
            $("#ExamAnswerToShow").prop('checked', false);
       
            $('#ExamAnswerToShow').removeAttr('disabled');

            $("#exammode12 .toggle-switch-checkbox").attr('checked', false);
            $("#ExamTimeType").prop('checked', false);
            $(".ExamTitleSpan").text('');
            $("#ErrorMsg").text('');
            $("#QuesTypeErr").text("");
            $(".NoOfQuesSpan").text('');
            CustomIDArray = [];
            totalQuestionArray = [];
            markquestionArray = [];
            incorrectasnwerArray = [];
            examskipquestionArray = [];
            $("#spanValue_0").text(Total);
            $("#spanValue_1").text(Skip);
            $("#spanValue_2").text(Incorrect);
            $("#spanValue_3").text(Marked);


        }
        else {
            $('#custommode').show();
            $('#anothermode').show();
            $('#custom').show();
            $('#quick').show();
            $('#questionfilter_0').attr('checked', false);
            $('#questionfilter_1').attr('checked', false);
            $('#questionfilter_2').attr('checked', false);
            $('#questionfilter_3').attr('checked', false); // Unchecks it
            
            //remove attribute disabled
            $('#questionfilter_1').removeAttr('disabled');
            $("#questionfilter_2").removeAttr('disabled');
            $("#questionfilter_3").removeAttr('disabled');
            $("#QuesTypeErr").text("");
            $("#NoofQuestions").val('');
            $('#ExamModeDiv').show();
            $(".SimulatedTextSpan").text("");
            $("#NoofQuestions").attr('disabled', false);
            $('#ExamName').val("Exam_custom_mode_" + custom_Mode_Count);
            $("#exammode12 .toggle-switch-checkbox").attr('checked', false);
            $("#ExamTimeType").prop('checked', false);
            $("#ExamAnswerToShow").prop('checked', true);
            $(".ExamTitleSpan").text('');
            $(".NoOfQuesSpan").text('');
            $("#ErrorMsg").text('');
            $("#ExamTypeTimeDiv").show();
            $("#spanValue_0").text(0);
            $("#spanValue_1").text(0);
            $("#spanValue_2").text(0);
            $("#spanValue_3").text(0);
            $('.TypeofCategoryList').attr('checked', false);
            $('#selectall').prop('checked', true);
            $("#selectall").trigger('change');        
        }
        });
        //var str1 = window.location.href;
        //var str2 = "QuickExam"
        //if (str1.indexOf(str2) != -1) {
        //    $('#ExamType').val('2');
        //    $('#ExamType').trigger('change', [{ value: 1 }]);
        //}
    Array.prototype.forEach.call(document.querySelectorAll('.clearable-input>[data-clear-input]'), function (el) {
        el.addEventListener('click', function (e) {
            e.target.previousElementSibling.value = '';
        });
    });

    $("#selectall").change(function () {
        //"select all" change
        $('#ErrorMsg').text("");
        $(".TypeofCategoryList").prop('checked', $("#selectall").prop("checked")); //change all ".checkbox" checked status
        if ($("#selectall").prop("checked")) {
            //$("#spanValue_0").text(Total);
            //$("#spanValue_1").text(Skip);
            //$("#spanValue_2").text(Incorrect);
            //$("#spanValue_3").text(Marked);
            var sList1 = "";
            var sList2 = "";
            $(".TypeofCategoryList").each(function () {
                var arrTotalQuestion = totalQuestionArray;
                var arrmarkquestion = markquestionArray;
                var arrincorrectasnwer = incorrectasnwerArray;
                var arrexamskipquestion = examskipquestionArray;
                var arr = CustomIDArray;

                var checkedId = $(this).attr('value');
                var totalquestioncountId = $(this).attr('totalcount');
                var markquestioncountId = $(this).attr('markquestioncount');
                var incorrectasnwercountId = $(this).attr('incorrectasnwercount');
                var examskipquestioncountId = $(this).attr('examskipquestioncount');

                //alert(checkedId + " " + totalquestioncountId + " " + markquestioncountId + " " + incorrectasnwercountId + " " + examskipquestioncountId);
                CustomIDArray.push(checkedId);
                totalQuestionArray.push(totalquestioncountId);
                markquestionArray.push(markquestioncountId);
                incorrectasnwerArray.push(incorrectasnwercountId);
                examskipquestionArray.push(examskipquestioncountId);

                arr = CustomIDArray;
                arrTotalQuestion = totalQuestionArray;
                arrmarkquestion = markquestionArray;
                arrincorrectasnwer = incorrectasnwerArray;
                arrexamskipquestion = examskipquestionArray;

                var allids = "";
                var allsum = 0;
                jQuery.each(totalQuestionArray, function (i, item) {
                    if (allids == "") {
                        allids = totalQuestionArray[i];
                    } else {
                        allids = allids + "," + totalQuestionArray[i];
                    }
                });
                var makrkids = "";
                var marksum = 0;
                jQuery.each(markquestionArray, function (i, item) {
                    if (makrkids == "") {
                        makrkids = markquestionArray[i];
                    } else {
                        makrkids = makrkids + "," + markquestionArray[i];
                    }
                });
                var incorrectids = "";
                var incorrectsum = 0;
                jQuery.each(incorrectasnwerArray, function (i, item) {
                    if (incorrectids == "") {
                        incorrectids = incorrectasnwerArray[i];
                    } else {
                        incorrectids = incorrectids + "," + incorrectasnwerArray[i];
                    }
                });
                var examskipquestionids = "";
                var examskipquestionsum = 0;
                jQuery.each(examskipquestionArray, function (i, item) {
                    if (examskipquestionids == "") {
                        examskipquestionids = examskipquestionArray[i];
                    } else {
                        examskipquestionids = examskipquestionids + "," + examskipquestionArray[i];
                    }
                });

                for (var i = 0; i < totalQuestionArray.length; i++) {
                    allsum += totalQuestionArray[i] << 0;
                }

                for (var i = 0; i < markquestionArray.length; i++) {
                    marksum += markquestionArray[i] << 0;
                }

                for (var i = 0; i < incorrectasnwerArray.length; i++) {
                    incorrectsum += incorrectasnwerArray[i] << 0;
                }

                for (var i = 0; i < examskipquestionArray.length; i++) {
                    examskipquestionsum += examskipquestionArray[i] << 0;
                }
              

            });

        } else {
           
            //$("#spanValue_0").text(0);
            //$("#spanValue_1").text(0);
            //$("#spanValue_2").text(0);
            //$("#spanValue_3").text(0);
            CustomIDArray = [];
            totalQuestionArray = [];
            markquestionArray = [];
            incorrectasnwerArray = [];
            examskipquestionArray = [];
        }

    });





    $(".TypeofCategoryList").change(function () {
        var sectionList =[];
        $.each($("input[name='TypeofCategoryList']:checked"), function () {
            sectionList.push($(this).val());
        });
        sectionValue = sectionList.join(", ");
        if (sectionValue == "") {
            sectionValue = 0;
        }       
        $.ajax({
            type: 'POST',
            data: { sectionValue: sectionValue },
            url: '/ExamManager/GetQuestionTypeCountBySection',
            async: false,
            success: function (result) {
                $("#spanValue_0").text(0);
                $("#spanValue_1").text(0);
                $("#spanValue_2").text(0);
                $("#spanValue_3").text(0);
                $("#spanValue_0").text(result.TotalCount);
                $("#spanValue_1").text(result.SkipCount);
                $("#spanValue_2").text(result.InCorrectCount);
                $("#spanValue_3").text(result.MarkCount);
           

                if ($("#spanValue_0").text() == 0) {
                    $("#questionfilter_0").attr('disabled', true);

                }
                else {
                    $("#questionfilter_0").removeAttr('disabled');
                }
                if ($("#spanValue_1").text() == 0) {
                    $("#questionfilter_1").attr('disabled', true);
                }
                else {
                    $("#questionfilter_1").removeAttr('disabled');
                }
                if ($("#spanValue_2").text() == 0) {
                    $("#questionfilter_2").attr('disabled', true);
                }
                else {
                    $("#questionfilter_2").removeAttr('disabled');
                }
                if ($("#spanValue_3").text() == 0){

                    $("#questionfilter_3").attr('disabled', true);
                }
                else{
                    $("#questionfilter_3").removeAttr('disabled');
                }
                
            }
        });
    });

    $(".TypeofQuestionList").change(function () {
        $("#QuesTypeErr").text("");
        // var CurrentId = $(this).attr('id');
        // var TotalCount = $('#' + CurrentId).attr("totalCount");
        var CurrentId = $(this).attr('value');
        var TotalCount = parseInt($('#spanValue_' + CurrentId).html());
        if (TotalCount == 0) {
            $('#questionfilter_' + CurrentId).attr('checked', false);
           
        }
        
   });

    $("#ExamName").on('keypress', function () {
     $(".ExamTitleSpan").text('');
    });
  $("#NoofQuestions").keypress(function (e) {
     $(".NoOfQuesSpan").text('');
    });
 $('#NoofQuestions').on('input', function (event) {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
$('.TypeofCategoryList').change(function () {
        //uncheck "select all", if one of the listed checkbox item is unchecked
        if (false == $(this).prop("checked")) { //if this item is unchecked
            $("#selectall").prop('checked', false); //change "select all" checked status to false
        }
        //check "select all" if all checkbox items are checked
        if ($('.TypeofCategoryList:checked').length == $('.TypeofCategoryList').length) {
        $("#selectall").prop('checked', true);
        }
    });


$('#questionfilter_0').change(function () {
   
        if (true == $(this).prop("checked")) {
            $('#questionfilter_1').attr('checked', false);
            $('#questionfilter_2').attr('checked', false);
            $('#questionfilter_3').attr('checked', false); // Unchecks it

        } else {
            $('#questionfilter_1').attr('disabled', false);
            $('#questionfilter_2').attr('disabled', false);
            $('#questionfilter_3').attr('disabled', false);
            }

    });

          $("#questionfilter_1").on('change', function () {
          $("#questionfilter_0").attr('checked', false);
       
    })

    $("#questionfilter_2").on('change', function () {
       $("#questionfilter_0").attr('checked', false)
    })

     $("#questionfilter_3").on('change', function () {
      $("#questionfilter_0").attr('checked', false)
     })

     $("#Create").click(function () {
         var totalSum = 0;
         $('#ExamAnswerToShow').removeAttr('disabled');
        var flag = true;
         if ($("#ExamName").val() == "") {
            $("#ExamName").focus();
            var ExamTitleMsg = "Please enter a unique exam title."
            $(".ExamTitleSpan").text(ExamTitleMsg);
            flag = false;
            return false;
         }
         if ($("#questionfilter_0").prop("checked")) {
             totalSum = 0;
             totalSum = parseInt($('#spanValue_0').html());
         }
         else {
             totalSum = 0;
             if ($("#questionfilter_1").prop("checked")) {
                 totalSum += parseInt($('#spanValue_1').html());
             }
             if (true == $("#questionfilter_2").prop("checked")) {
                 totalSum += parseInt($('#spanValue_2').html());
             }
             if (true == $("#questionfilter_3").prop("checked")) {
                 totalSum += parseInt($('#spanValue_3').html());
             }
         }
         if ($('#custommode:visible').length != 0 && $('#anothermode:visible').length != 0 && $('#quick:visible').length != 0 && $('#custom:visible').length != 0)
         {
             if ($('.rt [name=TypeofCategoryList]:checkBox:checked').length == 0) {
                 $('#selectall').focus();
                 var msg = "Please select at least one BCSC section."
                 $("#ErrorMsg").text(msg);
                 flag = false;
                 return false;
             }
             if ($("#NoofQuestions").val() == "") {
                 $("#NoofQuestions").focus();
                 var QuesErrMsg = "Please enter number of questions."
                 $(".NoOfQuesSpan").text(QuesErrMsg);
                 flag = false;
                 return false;
             }                                                
                 if ($("#NoofQuestions").val() > totalSum && (($("#questionfilter_0").prop("checked")==true) || ($("#questionfilter_1").prop("checked")==true) || ($("#questionfilter_2").prop("checked")==true) ||($("#questionfilter_3").prop("checked")==true)))
                 {
                     $("#NoofQuestions").focus();
                     var QuesErrMsg = "Number of question should not be greater than selected question type."
                     $(".NoOfQuesSpan").text(QuesErrMsg);
                     flag = false;
                     return false;
                 }                                     

            if ($("#NoofQuestions").val() == 0) {
                $("#NoofQuestions").focus();
                var QuesErrMsg = "Number of question should be greater than zero."
                $(".NoOfQuesSpan").text(QuesErrMsg);
                flag = false;
                return false;
            }

            if ($('.rtt [name=TypeofQuestionList]:checkBox:checked').length == 0) {
                var MsgText = "Please select at least one question type."
                $("#QuesTypeErr").text(MsgText);
                flag = false;
                return false;
            }
        }
        else if ($('#anothermode:visible').length != 0 && $('#custom:visible').length != 0) {
        
            if ($("#NoofQuestions").val() == "") {
                $("#NoofQuestions").focus();
                var QuesErrMsg = "Please enter number of questions."
                $(".NoOfQuesSpan").text(QuesErrMsg);
                flag = false;
                return false;
            }

            if ($("#NoofQuestions").val() == 0) {
                $("#NoofQuestions").focus();
                var QuesErrMsg = "Number of question should be greater than zero."
                $(".NoOfQuesSpan").text(QuesErrMsg);
                flag = false;
                return false;
            }

            if ($("#NoofQuestions").val() > totalSum && (($("#questionfilter_0").prop("checked") == true) || ($("#questionfilter_1").prop("checked") == true) || ($("#questionfilter_2").prop("checked") == true) || ($("#questionfilter_3").prop("checked") == true))) {
                $("#NoofQuestions").focus();
                var QuesErrMsg = "Number of question should not be greater than selected question type."
                $(".NoOfQuesSpan").text(QuesErrMsg);
                flag = false;
                return false;
            }

            if ($('.rtt [name=TypeofQuestionList]:checkBox:checked').length == 0) {

                var MsgText = "Please select at least one question type."
                $("#QuesTypeErr").text(MsgText);
                flag = false;
                return false;
            }
        }
        if (flag == true)
        {          
            $("Create").prop('disabled', true);           
        }
       
    });
    $('#ExamName').blur(function () {
        var url = "/ExamManager/CheckExamNameAvailable";
        var name = $('#ExamName').val();
        $(".SameNameErrSpan").text('');
        $("#Create").attr('disabled', false);
        $.get(url, { ExamName: name }, function (data) {
            if (data != "Available") {

                var CommonNameMsg = "Please enter a unique exam title"
                $(".SameNameErrSpan").text(CommonNameMsg);
                $(".ExamTitleSpan").text('');
                $("#ExamName").focus();
                $("#Create").attr('disabled', true);
            }
        });
    })
    // Total Number of Question count Code
    //increment and decremrnt QuestionType count on base of category selection
    $(".TypeofCategoryList").click(function (e) {
        var arrTotalQuestion = totalQuestionArray;
        var arrmarkquestion = markquestionArray;
        var arrincorrectasnwer = incorrectasnwerArray;
        var arrexamskipquestion = examskipquestionArray;
        var arr = CustomIDArray;

        var checkedId = $(this).attr('value');
        var totalquestioncountId = $(this).attr('totalcount');
        var markquestioncountId = $(this).attr('markquestioncount');
        var incorrectasnwercountId = $(this).attr('incorrectasnwercount');
        var examskipquestioncountId = $(this).attr('examskipquestioncount');

        if ($(this).prop('checked')) {
            CustomIDArray.push(checkedId);
            totalQuestionArray.push(totalquestioncountId);
            markquestionArray.push(markquestioncountId);
            incorrectasnwerArray.push(incorrectasnwercountId);
            examskipquestionArray.push(examskipquestioncountId);

            arr = CustomIDArray;
            arrTotalQuestion = totalQuestionArray;
            arrmarkquestion = markquestionArray;
            arrincorrectasnwer = incorrectasnwerArray;
            arrexamskipquestion = examskipquestionArray;

        } else {
            jQuery.each(CustomIDArray, function (i, item) {
                if (CustomIDArray[i] == checkedId) {
                    arr.splice(i, 1);
                    arrTotalQuestion.splice(i, 1);
                    arrmarkquestion.splice(i, 1);
                    arrincorrectasnwer.splice(i, 1);
                    arrexamskipquestion.splice(i, 1);
                }
            });
            CustomIDArray = arr;
            totalQuestionArray = arrTotalQuestion;
            markquestionArray = arrmarkquestion;
            incorrectasnwerArray = arrincorrectasnwer;
            examskipquestionArray = arrexamskipquestion;
        }
        var allids = "";
        var allsum = 0;
        jQuery.each(totalQuestionArray, function (i, item) {
            if (allids == "") {
                allids = totalQuestionArray[i];
            } else {
                allids = allids + "," + totalQuestionArray[i];
            }
        });
        var makrkids = "";
        var marksum = 0;
        jQuery.each(markquestionArray, function (i, item) {
            if (makrkids == "") {
                makrkids = markquestionArray[i];
            } else {
                makrkids = makrkids + "," + markquestionArray[i];
            }
        });
        var incorrectids = "";
        var incorrectsum = 0;
        jQuery.each(incorrectasnwerArray, function (i, item) {
            if (incorrectids == "") {
                incorrectids = incorrectasnwerArray[i];
            } else {
                incorrectids = incorrectids + "," + incorrectasnwerArray[i];
            }
        });
        var examskipquestionids = "";
        var examskipquestionsum = 0;
        jQuery.each(examskipquestionArray, function (i, item) {
            if (examskipquestionids == "") {
                examskipquestionids = examskipquestionArray[i];
            } else {
                examskipquestionids = examskipquestionids + "," + examskipquestionArray[i];
            }
        });

        for (var i = 0; i < totalQuestionArray.length; i++) {
            allsum += totalQuestionArray[i] << 0;
        }

        for (var i = 0; i < markquestionArray.length; i++) {
            marksum += markquestionArray[i] << 0;
        }

        for (var i = 0; i < incorrectasnwerArray.length; i++) {
            incorrectsum += incorrectasnwerArray[i] << 0;
        }

        for (var i = 0; i < examskipquestionArray.length; i++) {
            examskipquestionsum += examskipquestionArray[i] << 0;
        }
       

     
    });
    $("#Cancel").on('click', function () {
        if ($("#ExamType").val() == "0") {
            $("#NoofQuestions").val('');
            $('#selectall').attr('checked', false);
            $('.TypeofCategoryList').attr('checked', false);
            $('#questionfilter_0').attr('checked', false);
            $('#questionfilter_1').attr('checked', false);
            $('#questionfilter_2').attr('checked', false);
            $('#questionfilter_3').attr('checked', false); // Unchecks it
            $('#ExamTimeType').prop('checked', false);
            $("#ExamAnswerToShow").prop('checked', false);
            $("#exammode12 .toggle-switch-checkbox").attr('checked', false);
            $(".ExamTitleSpan").text('');
            $("#ErrorMsg").text('');
            $(".NoOfQuesSpan").text('');
        }
        else {
            $('#selectall').attr('checked', false);
            $('.TypeofCategoryList').attr('checked', false);
            $('#questionfilter_0').attr('checked', false);
            $('#questionfilter_1').attr('checked', false);
            $('#questionfilter_2').attr('checked', false);
            $('#questionfilter_3').attr('checked', false); // Unchecks it
            $('#ExamTimeType').prop('checked', false);
            $("#ExamAnswerToShow").prop('checked', false);
            $("#exammode12 .toggle-switch-checkbox").attr('checked', false);
            $(".ExamTitleSpan").text('');
            $("#ErrorMsg").text('');
            $(".NoOfQuesSpan").text('');
        }
      
    });

    var CurrentId = $(this).attr('value');
    var TotalCount = parseInt($('#spanValue_' + CurrentId).html());
    $('#ExamType').val('2');
  //$('#ExamType').val(0).trigger('change');
  //  var str1 = window.location.href;
  //  var str2 = "QuickExam"
  //  if (str1.indexOf(str2) != -1) {
  //     $('#ExamType').val('2');
  //     $('#ExamType').trigger('change', [{ value: 2 }]);
  // } else {
  //     $('#ExamType').val(0).trigger('change');
  //  }
})();