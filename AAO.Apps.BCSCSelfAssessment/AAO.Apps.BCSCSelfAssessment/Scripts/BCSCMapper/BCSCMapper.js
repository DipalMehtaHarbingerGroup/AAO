$(document).ready(function () {

    $("#SearchByTopli").on('click', function () {
        $("div[id$='collapseOne']").removeClass("in");
        $("div[id$='collapseOne']").removeAttr("style");
    });

    $("#SearchByIdTitle").on('click', function () {
        $("div[id$='collapseOne']").removeClass("in");
        $("div[id$='collapseOne']").removeAttr("style");
    });


    $("#searchOnIDTitle").prop("disabled", true);
    $("#searchByQuestion").show();
    if ($("#search").val() != "") {
        $("#searchByQuestion").show();
        $("#search").prop("disabled", true);
        $("#questionTitle").prop('checked', true);
    }

    var Id = "ID";
    var title = "Title";

    if (BcscMapperViewModel.ProgramId != '') {
        $("#topPanel").append($("#searchBySecChaptTop"));
        $("#SearchByTopli").addClass('active');
        $("#SearchByIdTitle").removeClass('active');
        $("#searchByQuest").prop("disabled", true);
        $("#searchByQuestion").hide();

        $("#SearchByIdTitle").on('click', function () {
            $("#searchByQuestion").show();
            $("#searchBySecChaptTop").hide();
            var noOfRecords = $('#QuestViewPerPage option:selected').val();
            window.location.href = '/BCSCMapper/Index' + '?pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords;
        });

        $("#SearchByTopli").on('click', function () {
            $("#searchByQuestion").hide();
            $("#searchBySecChaptTop").show();
            var noOfRecords = $('#QuestViewPerPage option:selected').val();
            window.location.href = '/BCSCMapper/Index' + '?pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords;
        });
    }


    var pageIndex = BcscMapperViewModel.PageNo;
    if(BcscMapperViewModel.QuestionText != '') {
        $("#search").val(BcscMapperViewModel.QuestionText);
        $("#search").prop("disabled", true);
        $("#questionTitle").prop('checked', true);
        $("input[type=radio][value=" + Id + "]").attr("disabled", true);
        $("input[type=radio][value=" + title + "]").attr("disabled", true);
    }
    else if(BcscMapperViewModel.QuestionId != '') {
        $("#search").val(BcscMapperViewModel.QuestionId);
        $("#search").prop("disabled", true);
        $("#questionId").prop('checked', true);
        $("input[type=radio][value=" + title + "]").attr("disabled", true);
        $("input[type=radio][value=" + Id + "]").attr("disabled", true);
    }


    pagination(pageIndex);
    function pagination(PageNoVal) {
        var totalnoofitem = BcscMapperViewModel.questionCount;
        var noOfRecords = BcscMapperViewModel.noOfRecords;
        $("#QuestViewPerPage").val(noOfRecords).change();
        var limitperpage = $('#QuestViewPerPage option:selected').val();
        var totalpages = Math.round(totalnoofitem / limitperpage);
        var RoundVal = (totalnoofitem / limitperpage);
        RoundVal = RoundVal.toFixed(2);
        var output = RoundVal.split('.')[0];
        RoundVal = RoundVal - output;
        if (RoundVal > 0 && RoundVal < 0.50)
            RoundVal = 1
        else
            RoundVal = 0
        totalpages = totalpages + RoundVal;
        if (PageNoVal == undefined) {
            PageNoVal = 1;

        }

        for (var i = 1; i <= totalpages; i++) {
            if(i == PageNoVal) {
                $(".pagination").append("<li  class='currentpage active' id='" + i + "Page'><a href='javascript:void(0)'>" + i + "</a></li>");
            }
            else{
                if(i <= 10) {
                    $(".pagination").append("<li class='currentpage' id='" + i + "Page'><a href='javascript:void(0)'>" + i + "</a></li>");
                }
                else{
                    $(".pagination").append("<li class='currentpage hidden'><a href='javascript:void(0)'>" + i + "</a></li>");
                }

            }
        }

        $(".pagination").append("<li id='Nextpage'> <a  href='javascript:void(0)'>Next</a></li>");
        $(".pagination").append("<li id='Lastpage'> <a id='Last' href='javascript:void(0)' TotalPages='" + totalpages + "'>Last</a></li>");

        if(totalpages <= 10) {
            $("#Lastpage").hide();
            $("#Firstpage").hide();
        }

        $(".pagination li.currentpage").on("click", function () {
            if($(this).hasClass("active")) {
                return false;
            }
            else{
                var currentpage = $(this).index();
                currentpage = (currentpage - 1);
                var getdata = reloadQuestions(currentpage);
                $(".pagination li").removeClass("active");
                $(this).addClass("active");
            }

        });
        $("#Nextpage").on("click", function () {
            var currentpage = $(".pagination li.active").index();
            if(currentpage > totalpages) {
                return false;
            }
            else{
                reloadQuestions(currentpage);
            }
        });
        $("#Previouspage").on("click", function () {
            var currentpage = $(".pagination li.active").index();
            if(currentpage === 2) {
                return false;
            }else if(currentpage >= 1) {
                currentpage = currentpage - 2;
                reloadQuestions(currentpage);
            }
        });

        $("#Lastpage").on("click", function () {
            reloadQuestions(totalpages);
        });


        $("#Firstpage").on("click", function () {
            var currentpage = 1;
            reloadQuestions(currentpage);
        });
    }

    $("#QuestViewPerPage").on('change', function () {
        var PageNo = 1;
        reloadQuestions(PageNo);
    });


    function reloadQuestions(pageIndex) {
        var progId = BcscMapperViewModel.ProgramId;
        var noOfRecords = $('#QuestViewPerPage option:selected').val();
        var RadioButtonState = BcscMapperViewModel.RadioButtonState;
        if((RadioButtonState == 'Title')) {
            SearchByTitle(pageIndex, RadioButtonState);
        }

        else if(progId != '') {
            var pageIndex = pageIndex;
            SearchQuestions(pageIndex);
        }
        else {
            window.location.href = '/BCSCMapper/Index' + '?pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords;
        }
    }

    $("#resetQuestions").on('click', function () {
        window.location.href = '/BCSCMapper/Index';
    });

    $("#resetQuest").on('click', function () {
        window.location.href = '/BCSCMapper/Index';
    });

    $("#searchOnIDTitle").on('click', function () {
        var isChecked = $('input:radio[name="question"]').prop('checked');
        if(isChecked == "false") {
            alert("Please select search radio button first");
        }
    });


    $('input:radio[name="question"]').click(function () {
        $("#search").val('');
        if($('input:radio[name="question"]:checked')[0].value == "ID") {
            var changedVal = $("#search").attr("placeholder", "Search the question Id to map the BCSC content").placeholder();
            $('#search').on('input', function (event) {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }
    });

    $("#searchOnIDTitle").on('click', function () {
        if($('input:radio[name="question"]:checked')[0].value == "Title") {
            var RadioButtonState = BcscMapperViewModel.RadioButtonState;
            SearchByTitle(pageIndex, RadioButtonState);
        }
        else
            SearchByID();
    });

    $("#search").on("keypress", function (event) {
        if($('input:radio[name="question"]:checked')[0].value == "Title") {
            $("#search").attr('type', 'text');
        }
        else {
            if (event.keyCode >=48 && event.keyCode <= 57)
            {
                $("#search").attr('type', 'text');
            }
            else
            {
                return false;
            }

        }

        $("#searchOnIDTitle").removeAttr('disabled');
        if (event.which == 13) {
            if ($('input:radio[name="question"]:checked')[0].value == "Title") {
                var RadioButtonState = BcscMapperViewModel.RadioButtonState;
                SearchByTitle(pageIndex, RadioButtonState);
            }
            else
                SearchByID();
        }
    });


    function SearchByTitle(pageIndex, RadioButtonState) {
        var radioButtonState = $("#questionTitle").val();
        var pageIndex = pageIndex;
        var noOfRecords = $('#QuestViewPerPage option:selected').val();
        var regex = new RegExp("^[a-zA-Z]+$");
        var QuestionText = BcscMapperViewModel.QuestionText;
        if (QuestionText != '') {
            QuestionText = BcscMapperViewModel.QuestionText;
        }
        else{
            QuestionText = $("#search").val();
        }

        if(!($.isNumeric(QuestionText)) || (regex.test(QuestionText) == true)) {
            window.location.href = '/BCSCMapper/GetQuestionByTitle' + '?QuestionText=' + QuestionText + '&pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords + '&radioButtonState=' + radioButtonState;
        }
        else{
            alert("Please enter valid title");
            $("#search").val('');
            $('input:radio[name="question"]').prop('checked', false);
        }
    }


    function SearchByID() {
        var QuestionId = $("#search").val();
        if ($.isNumeric(QuestionId)) {
            window.location.href = '/BCSCMapper/GetQuestionById' + '?QuestionId=' + QuestionId;
        }
        else {
            alert("Please enter valid question ID");
            $("#search").val('');
            $('#questionId').removeAttr('value');
        }

    }

    $.ajax({
        type: 'POST',
        data: {},
        url: '/BCSCMapper/GetProgram',
        success: function (result) {
            for (var i = 0; i < result.length; i++) {

                if (BcscMapperViewModel.ProgramId > 0 && BcscMapperViewModel.ProgramId == result[i].programId) {
                    $("#PageNavigation").addClass("disable");
                    program = '<option value="' + result[i].programId + '" disabled selected hidden>' + result[i].program + '</option>';
                    $('#ProgramBox').html(program);
                    $('#ProgramBox').attr("disabled",true);
                }
                else {
                    program = '<option value="' + result[i].programId + '">' + result[i].program + '</option>';
                    $('#ProgramBox').append(program);
                }
            }
        }
    });

    $('#ProgramBox').on('change', function () {
        var ProgramId = "";
        $('#SubProgramBox').removeAttr('disabled');
        if (BcscMapperViewModel.ProgramId > 0) {
            ProgramId = BcscMapperViewModel.ProgramId;
            $.ajax({
                type: 'GET',
                data: { ProgramId: ProgramId },
                url: '/BCSCMapper/GetSubProgram',
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        subprogram = '<option value="' + result[i].subprogramId + '" disabled selected hidden>' + result[i].subprogram + '</option>';
                        $('#SubProgramBox').html(subprogram);
                        $('#SubProgramBox').attr("disabled", true);

                    }
                }
            });
        }
        else {
            ProgramId = $("#ProgramBox").val();
            $.ajax({
                type: 'GET',
                data: { ProgramId: ProgramId },
                url: '/BCSCMapper/GetSubProgram',
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        subprogram = '<option value="' + result[i].subprogramId + '">' + result[i].subprogram + '</option>';
                        $('#SubProgramBox').append(subprogram);

                    }
                }
            });
        }
    });


    if (BcscMapperViewModel.ProgramId > 0) {
        $('#ProgramBox').trigger('change');
    }

    $('#SubProgramBox').on('change', function () {
        var SubProgramId = "", subSpecialty = "", subSpecialityId = "";
        $('#SubSpecialityBox').removeAttr('disabled');
        if (BcscMapperViewModel.SubprogramId > 0) {
            subSpecialityId = BcscMapperViewModel.SubSpecialityId;
            $.ajax({
                type: 'GET',
                data: { subSpecialityId: subSpecialityId },
                url: '/BCSCMapper/GetSubSpecFromSpecialityId',
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        subSpecialty += '<option value="' + result[i].SubspecialtyId + '" disabled selected hidden>' + result[i].Subspecialty + '</option>';
                        $('#SubSpecialityBox').html(subSpecialty);
                        $("#PageNavigation").removeClass("disable");
                    }
                }
            });
        }
        else {
            SubProgramId = $('#SubProgramBox option:selected').val();
            $.ajax({
                type: 'GET',
                data: { SubProgramId: SubProgramId },
                url: '/BCSCMapper/GetSubSpeciality',
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        subSpecialty += '<option value="' + result[i].SubspecialtyId + '">' + result[i].Subspecialty + '</option>';
                    }
                    $('#SubSpecialityBox').append(subSpecialty);
                }
            });
        }
    });

    if (BcscMapperViewModel.SubprogramId > 0) {
        $('#SubProgramBox').trigger('change');
    }

    $('#SubSpecialityBox').on('change', function () {
        var SubSpecialityId = "";
        $('#SearchQuestions').removeAttr('disabled');
        $('#TopicBox').removeAttr('disabled');
        $('#TopicBox option').remove();
        var Topic = "";
        if (BcscMapperViewModel.SubSpecialityId > 0) {
            TopicId = BcscMapperViewModel.TopicId;
            $.ajax({
                type: 'GET',
                data: { topicId: TopicId },
                url: '/BCSCMapper/GetTopicByTopicId',
                success: function (result) {
                    Topic = '<option value="-1" disabled selected hidden>' + "Select Topic" + '</option>';
                    for (var i = 0; i < result.length; i++) {
                        Topic += '<option value="' + result[i].TopicId + '" disabled selected hidden>' + result[i].TopicName + '</option>';
                        $('#TopicBox').html(Topic);
                        $("#PageNavigation").removeClass("disable");
                        $('#SubSpecialityBox').attr("disabled", true);
                        $('#TopicBox').attr("disabled", true);
                    }
                }
            });
        }
        else {
            SubSpecialityId = $('#SubSpecialityBox option:selected').val();
            $.ajax({

                type: 'GET',
                data: { subSpecialityId: SubSpecialityId },
                url: '/BCSCMapper/GetTopicBySubSpec',
                success: function (result) {
                    Topic = '<option value="-1" disabled selected hidden>' + "Select Topic" + '</option>';
                    for (var i = 0; i < result.length; i++) {
                        Topic += '<option value="' + result[i].TopicId + '">' + result[i].TopicName + '</option>';
                    }
                    $('#TopicBox').append(Topic);
                    $('#SubSpecialityBox').attr("disabled", true);
                    //$('#TopicBox').attr("disabled", true);
                }
            });
        }



    });

    if (BcscMapperViewModel.SubSpecialityId > 0) {
        $('#SubSpecialityBox').trigger('change');
        $('#SubSpecialityBox').attr("disabled", true);
    }

    //Search questions based on SubSpeciality and Topic
    $('#SearchQuestions').on('click', function () {
        pageIndex = 1;
        SearchQuestions(pageIndex);
    });

    function SearchQuestions(pageIndex) {
        var pageIndex = pageIndex;
        if (pageIndex != undefined)
            pageIndex = pageIndex;
        else
            pageIndex = 1;
        var noOfRecords = $('#QuestViewPerPage option:selected').val();
        var ProgramId = $('#ProgramBox option:selected').val();
        var SubProgramId = $('#SubProgramBox option:selected').val();
        var SubSpecialtyId = $('#SubSpecialityBox option:selected').val();
        var TopicId = $('#TopicBox option:selected').val();
        if (TopicId == "Select Topic" || TopicId == "0" || TopicId =="-1") {
            TopicId = null;

            window.location.href = '/BCSCMapper/GetQuesByTopic' + '?topicId=' + TopicId + '&subSpecialtyId=' + SubSpecialtyId + '&programId=' + ProgramId + '&subProgramId=' + SubProgramId + '&pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords;
        }
        else {
            window.location.href = '/BCSCMapper/GetQuesByTopic' + '?topicId=' + TopicId + '&subSpecialtyId=' + SubSpecialtyId + '&programId=' + ProgramId + '&subProgramId=' + SubProgramId + '&pageIndex=' + pageIndex + '&noOfRecords=' + noOfRecords;
        }
    }


    $('.element').click(function () {
        $("div[id$='collapseInnerOne']").removeClass("in");

        var id = $(this).attr('value');
        $("#" + id + "collapseInnerOne").removeClass("in");
        if ($("#" + id + "ContainerDiv").html() == "") {
            $.ajax({
                type: 'GET',
                data: { questionId: id },
                url: '/BCSCMapper/GetQuestionContent',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != '') {
                        i = 0;
                        $.each(result, function (key, value) {
                            var data = value.program + "<i val='program' class='fa fa-angle-right marleft20 marright20'></i>"

                            data += value.subprogram + "<i val='subprogram' class='fa fa-angle-right marleft20 marright20'></i>"
                            data += value.Subspecialty + "<i val='Subspecialty' class='fa fa-angle-right marleft20 marright20'></i>"
                            data += value.TopicName;
                            var spanData = ("<div id='" + i + "quesDetails' class='nested-accordion-title'>" + data +
                                "<a id='" + id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton' class='accordion-toggle' questionId='" + id + "' TopicId='" + value.TopicId + "' speciality='" + value.Subspecialty + "' TopicId='" + value.TopicId + "' specialityId='" + value.SubspecialtyId + "' data-toggle='collapse' data-parent='#accordion' href='" + id + "collapseInnerOne'>Update Content </a> " + "</div > ");


                            $("#" + id + "ContainerDiv").append(spanData);
                            $.ajax({
                                type: 'GET',
                                data: { questionId: id, subSpecialityId: value.SubspecialtyId, topicId: value.TopicId },
                                url: '/BCSCMapper/GetButtonStatus',
                                success: function (response) {
                                    if (response == true) {
                                        document.getElementById(id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton").innerHTML = "Update Content ";
                                        document.getElementById(id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton").id = id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton";
                                        BindUpdateClickEvent(id, value.SubspecialtyId, value.TopicId);

                                    }
                                    else {
                                        document.getElementById(id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton").innerHTML = "Map Content ";
                                        document.getElementById(id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "updateButton").id = id + "_" + value.SubspecialtyId + "_" + value.TopicId + "_" + "mapButton";
                                        BindMapClickEvent(id, value.SubspecialtyId, value.TopicId);

                                    }
                                }
                            });

                            i++;
                        })
                    }
                }
            });
        }
    });


    var stringSpeciality = "";
    var stringUpdateSpeciality = "";

    function BindMapClickEvent(id, SubspecialtyId, TopicId) {

        //On click of Map button
        var topicId = TopicId
        $("#QuestionList").on('click', "#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "mapButton", function () {
            $("#" + questionId + "ChapterName option").remove();
            var idVal = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "mapButton").attr('topicId');
            $("#" + id + "alertMessage").hide();
            if (stringSpeciality.indexOf(idVal) == '-1' ) {
                var newId = id + "_" + idVal + "collapseInnerOne";
                $("#" + id + "_" + stringSpeciality + "collapseInnerOne").attr('id', newId);
                stringSpeciality = idVal;

                if (!($("#" + id + "collapseInnerOne").hasClass("accordion-toggle collapse in"))) {
                    $("#" + id + "listData").empty();
                    $("#" + id + "discussionPara").empty();
                    $("#" + id + "referencesPara").empty();
                    $("#" + id + "relatedContentsPara").empty();
                    $("div[id$='collapseInnerOne']").removeAttr("style");
                    $("div[id$='collapseInnerOne']").removeClass("in");
                    var newId = id + "_" + idVal + "collapseInnerOne";
                    $("#" + id + "collapseInnerOne").attr('id', newId);
                    $("#" + newId).addClass("in");

                    var questionId = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "mapButton").attr('questionId');
                    var subSpecialityId = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "mapButton").attr('specialityId');
                    var speciality = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "mapButton").attr('speciality');

                    //Display details related to question
                    displayQuestDetails(questionId, subSpecialityId);

                    //On click save text
                    saveContent(questionId, topicId);

                    $("#" + questionId + "SubSpecName").find('option:selected').remove();
                    $("#" + questionId + "SubSpecName").append($('<option value="' + subSpecialityId + '">' + speciality + '</option>'));
                    tinymce.get(questionId + "descriptionArea").setContent("");
                    tinymce.get(questionId + "descriptionArea1").setContent("");
                    //$("#" + questionId + "descriptionArea1").empty();
                    $("#" + questionId + "ChapterName").empty();

                    //Fill chapter names in chapter listbox
                    var chapter = "";
                    $.ajax({
                        type: 'GET',
                        data: { subSpecialityId: subSpecialityId },
                        url: '/BCSCMapper/GetChapter',
                        success: function (result) {
                            chapter += '<option value="-1" disabled selected hidden>' + "Select Chapter Name" + '</option>';
                            for (var i = 0; i < result.length; i++) {
                                chapter += '<option value="' + i.toString() + '">' + result[i].ChapterName + '</option>';
                            }

                            $("#" + questionId + "ChapterName").append(chapter);
                        }
                    });


                    //Fill descriptionArea on change of chapter listbox
                    $("#" + questionId + "ChapterName").on('change', function () {
                        $("#" + questionId + "alertMessage").hide();
                        var ChapterName = $("#" + questionId + "ChapterName option:selected").text();
                        var SubSpecId = $("#" + questionId + "SubSpecName").val();

                        tinymce.get(questionId + "descriptionArea").setContent('');
                        //$("#" + questionId + "descriptionArea").empty();

                        //Used to fill Description area
                        getBCSCHTMLData(SubSpecId, ChapterName, questionId);
                    });

                    //On click transfer data to descriptionArea1
                    copySelected(questionId)

                    //On click clear text
                    clearContent(questionId);
                }
            }
            else {

                stringSpeciality = "";
                {
                    var newId = id + "_" + idVal + "collapseInnerOne";
                    var nId = $("#" + "collapseInnerOne").attr('id', newId);
                    $("#" + id + "collapseInnerOne").removeClass("in");
                    $("div[id$='collapseInnerOne']").removeAttr("style");
                    $("div[id$='collapseInnerOne']").find(".collapse").removeClass("in");
                    var newId = id + "collapseInnerOne";
                    var nId = $("#" + id + "_" + idVal + "collapseInnerOne").attr('id', newId);
                    $("#" + id + "collapseInnerOne").removeClass("in");
                }
            }
        });
    }

    function BindUpdateClickEvent(id, SubspecialtyId, TopicId) {
        //On click of Update button
        var topicId = TopicId;
        $("#QuestionList").on('click', "#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "updateButton", function () {
            var idVal = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "updateButton").attr('topicId');
            $("#" + id + "alertMessage").hide();
            if (stringSpeciality.indexOf(idVal) == '-1') {
                var newId = id + "_" + idVal + "collapseInnerOne";
                $("#" + id + "_" + stringSpeciality + "collapseInnerOne").attr('id', newId);
                stringSpeciality = idVal;

                if (!($("#" + id + "collapseInnerOne").hasClass("collapse in"))) {
                    $("#" + id + "listData").empty();
                    $("#" + id + "discussionPara").empty();
                    $("#" + id + "referencesPara").empty();
                    $("#" + id + "relatedContentsPara").empty();
                    $("div[id$='collapseInnerOne']").removeAttr("style");
                    $("div[id$='collapseInnerOne']").removeClass("in");
                    var newId = id + "_" + idVal + "collapseInnerOne";
                    $("#" + id + "collapseInnerOne").attr('id', newId);
                    $("#" + newId).addClass("in");
                    $("#" + id + "LastUpdateOn_IsUpdated").empty();
                    $("#" + id + "LastUpdateOn_BookUpdateAPI").empty();
                    
                    var questionId = id;
                    var subSpecialityId = SubspecialtyId;
                    var speciality = $("#" + id + "_" + SubspecialtyId + "_" + TopicId + "_" + "updateButton").attr('speciality');

                    //used to display Choices, Discussion and References
                    displayQuestDetails(questionId, subSpecialityId);


                    //used to update data
                    saveContent(questionId, topicId);

                    tinymce.get(questionId + "descriptionArea1").setContent("");
                    //$("#" + questionId + "descriptionArea1").empty();


                    //Used to get all saved data and append to listbox and description area1
                    var QuestionId = "";
                    var SpecialityId = "";
                    var ChapterName = "";
                    var content = "";
                    var LastUpdateOn_IsUpdated = "";
                    var LastUpdateOn_BookUpdateAPI = "";
                    $.ajax({
                        type: 'POST',
                        data: { questionId: questionId, subSpecialityId: subSpecialityId, topicId: topicId },
                        url: '/BCSCMapper/GetUpdateContent',
                        contentType: "application/x-www-form-urlencoded",
                        dataType: "json",
                        success: function (result) {

                            QuestionId = result[0].QuestionId;
                            SpecialityId = result[0].SubspecialtyId;
                            ChapterName = result[0].ChapterName;
                            content += result[0].Content;
                            //content = decodeURIComponent(content);
                            content = content;
                            tinymce.get(questionId + "descriptionArea1").setContent(content);

                            LastUpdateOn_IsUpdated = result[0].LastUpdateOn_IsUpdated;
                            LastUpdateOn_BookUpdateAPI = result[0].LastUpdateOn_BookUpdateAPI;
                            $("#" + questionId + "LastUpdateOn_IsUpdated").append(LastUpdateOn_IsUpdated);
                            $("#" + questionId + "LastUpdateOn_BookUpdateAPI").append(LastUpdateOn_BookUpdateAPI);

                            $("#" + questionId + "SubSpecName").find('option:selected').remove();
                            var subspec = $("#" + questionId + "SubSpecName").html($('<option value="' + subSpecialityId + '">' + speciality + '</option>'));
                            var chapt = $("#" + questionId + "ChapterName").find('option:selected').remove();


                            var chapter = $("#" + questionId + "ChapterName").html($('<option>' + ChapterName + '</option>'));
                            var chapterListBoxval = $("#" + questionId + "ChapterName option:selected").text();

                            //Used to fill Description area
                            getBCSCHTMLData(subSpecialityId, ChapterName, questionId);


                            var chapter = "";
                            $.ajax({
                                type: 'GET',
                                data: { subSpecialityId: SpecialityId },
                                url: '/BCSCMapper/GetChapter',
                                success: function (result) {
                                    for (var i = 0; i < result.length; i++) {
                                        if (chapterListBoxval!= result[i].ChapterName) {
                                            chapter += '<option value="' + i.toString() + '">' + result[i].ChapterName + '</option>';
                                        }
                                        else {
                                            continue;
                                        }
                                    }

                                    $("#" + questionId + "ChapterName").append(chapter);
                                }
                            });


                            $("#" + questionId + "ChapterName").on('change', function () {

                                var ChapterName = $("#" + questionId + "ChapterName option:selected").text();
                                var SubSpecId = $("#" + questionId + "SubSpecName").val();
                                $("#" + questionId + "alertMessage").hide();
                                tinymce.get(questionId + "descriptionArea").setContent("");

                                //Used to fill Description area
                                getBCSCHTMLData(SubSpecId, ChapterName, questionId);
                            });


                            tinymce.get(questionId + "descriptionArea").setContent("");

                            //Used to transfer data to description area1
                            copySelected(questionId);

                            clearContent(questionId);
                            //Used to clear text

                        }

                    });
                }
            }
            else {
                stringSpeciality = "";
                {
                    var newId = id + "_" + idVal + "collapseInnerOne";
                    var nId = $("#" + "collapseInnerOne").attr('id', newId);
                    $("#" + id + "collapseInnerOne").removeClass("in");
                    $("div[id$='collapseInnerOne']").removeAttr("style");
                    $("div[id$='collapseInnerOne']").find(".collapse").removeClass("in");
                    var newId = id + "collapseInnerOne";
                    var nId = $("#" + id + "_" + idVal + "collapseInnerOne").attr('id', newId);
                    $("#" + id + "collapseInnerOne").removeClass("in");
                }

            }

        });
    }

    function saveContent(questionId, topicId) {
        var NewTopicId = topicId;
        $("#" + questionId + "SaveText").unbind('click');
        $("#" + questionId + "SaveText").click(function () {
            var textAreaData = tinymce.get(questionId + "descriptionArea1").getContent();
            if (textAreaData=='') {
                alert("Please add data before saving");
            }
            else {
                var QuestionId = questionId;
                var Content = tinymce.get(questionId + "descriptionArea1").getContent();
                if (Content.length <= 0) {
                    alert('Please remove empty space');
                    return false;
                }
                var subSpecialityId = $("#" + questionId + "SubSpecName").val();
                var ChapterName = $("#" + questionId + "ChapterName option:selected").text();
                Content = encodeURIComponent(Content);
                //Content = Content;
                $.ajax({
                    type: 'POST',
                    async: false,
                    data: { questionId: QuestionId, subSpecialityId: subSpecialityId, chapterName: ChapterName, topicId: topicId, contentData: Content },
                    url: '/BCSCMapper/InsertContent',
                    contentType: "application/x-www-form-urlencoded",
                    dataType: "json",
                    success: function (result) {
                        if (result == 1) {
                            $("#" + QuestionId + "alertMessage").show();
                            document.getElementById(QuestionId + "_" + subSpecialityId + "_" + topicId + "_" + "mapButton").innerHTML = "Update Content";
                            document.getElementById(QuestionId + "_" + subSpecialityId + "_" + topicId + "_" + "mapButton").id = QuestionId + "_" + subSpecialityId + "_" + topicId + "_" + "updateButton";

                            //$("div[id$='collapseInnerOne']").removeAttr("style");
                            //$("div[id$='collapseInnerOne']").removeClass("in");
                            BindUpdateClickEvent(QuestionId, subSpecialityId, topicId);
                        }
                        else {
                            alert("Problem occured while updating data");
                        }
                    }
                });
            }
        });

    }

    function clearContent(questionId) {
        $("#" + questionId + "ClearText").click(function () {
            tinymce.get(questionId + "descriptionArea1").setContent('');
            $("#" + questionId + "alertMessage").hide();

        });

    }

    function getSelectedText(el) {
        if (typeof el.selectionStart == "number") {
            return el.value.slice(el.selectionStart, el.selectionEnd);
        } else if (typeof document.selection != "undefined") {
            var range = document.selection.createRange();
            if (range.parentElement() == el) {
                return range.text;
            }
        }
        return "";
    }

    function copySelected(questionId) {

        $("#" + questionId + "Transfer").click(function () {
            $("#" + questionId + "alertMessage").hide();
            var value = tinymce.get(questionId + 'descriptionArea').selection.getContent({ format: 'html' });
            tinymce.get(questionId + 'descriptionArea1').selection.setContent(value);
            tinymce.get(questionId + 'descriptionArea').execCommand('mceCleanup');
        });
    }


    //Fill DescriptionArea with BCSC book content
    function getBCSCHTMLData(subSpecialityId, ChapterName, questionId) {

        $.ajax({
            type: 'GET',
            data: { subSpecialityId: subSpecialityId },
            url: '/BCSCMapper/GetBCSCSectionNumber',
            crossDomain: true,
            success: function (result) {
                var BCSCSectionNumber = result[0].BCSCSectionNumber;
                $.ajax({
                    type: 'GET',
                    data: { chapterName: ChapterName, bcscSectionNumber: BCSCSectionNumber },
                    url: '/BCSCMapper/GetHtmlPage',
                    success: function (result) {
                        var mydata = $("#loadPage").html(result);
                        if (BCSCSectionNumber == 3 || BCSCSectionNumber == 4 || BCSCSectionNumber == 10 || BCSCSectionNumber == 11 ||
                            BCSCSectionNumber == 5 || BCSCSectionNumber == 8 || BCSCSectionNumber == 13 || BCSCSectionNumber == 6 ||
                            BCSCSectionNumber == 12 || BCSCSectionNumber == 1 || BCSCSectionNumber == 2 || BCSCSectionNumber == 7 ||
                            BCSCSectionNumber == 9)
                        {
                            //var content = result.replace(/<img[^>]*>/g, "");
                            tinymce.get(questionId + "descriptionArea").setContent(result);
                        } else {
                            var content = result.replace(/<img[^>]*>/g, "");
                            tinymce.get(questionId + "descriptionArea").setContent(content);
                        }
                        // tinymce.get(questionId + "descriptionArea").setContent(result);

                        $("#loadPage").hide();
                    }

                });
            }

        });
    }


    //used to display Choices, Discussion and References
    function displayQuestDetails(questionId, subSpecialityId) {
        $.ajax({
            type: 'GET',
            data: { questionId: questionId, subSpecialityId: subSpecialityId },
            url: '/BCSCMapper/GetBCSCQuestDetails',
            success: function (result) {

                var choices = "";
                var disc = "";
                var ref = "";
                var rel = "";
                //console.log(result[0]);
                $.each(result[0].ChoiceList, function (key, value) {
                    if (result[0].RightChoiceId == value.choiceId)
                    {
                        choices += "<li class='bg-success'>" + value.choiceSequence + ". " + value.choiceText + "</li>";
                    }
                    else
                    {
                        choices += "<li>" + value.choiceSequence + ". " + value.choiceText + "</li>";
                    }

                });

                disc = result[0].Discussion;
                ref = result[0].References;
                rel = result[0].RelatedContents;
                var discussionId = questionId + "discussion";
                var referenceId = questionId + "reference";
                $("#" + questionId + "listData").append(choices);
                $("#" + questionId + "discussionPara").append(disc);
                $("#" + questionId + "referencesPara").append(ref);
                $("#" + questionId + "relatedContentsPara").append(rel);
            }
        });
    }
});