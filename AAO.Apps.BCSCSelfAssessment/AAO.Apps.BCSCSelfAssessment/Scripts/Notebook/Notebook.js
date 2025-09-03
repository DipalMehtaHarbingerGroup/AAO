$(document).ready(function () {
    $("#VersionInfo").append(navigator.appVersion);
    var numberOfRecords = NotebookViewModel.noOfRecords;
    var chapterCount = NotebookViewModel.chapterCount;
    var firstSerialNo = NotebookViewModel.firstSerialNo;
    var lastSerialNo = NotebookViewModel.lastSerialNo;
    var chapterName = NotebookViewModel.chapterName;
    var subspecilty = NotebookViewModel.subSpecialtyId;
    var pageIndex = NotebookViewModel.PageNo;

    //if ($("#details").val() == undefined)
    //$("#PageNavigation").hide();
    //else
    pagination(pageIndex);
    //$("#displayCount").append("Displaying questions " + firstSerialNo + " - " + lastSerialNo + " of " + chapterCount);

    //  tabs / accordion
    $('.responsive-tabs').responsiveTabs({
        accordionOn: ['xs', 'sm']
    });

    $('.EditText').hide();

    $.ajax({
        type: 'POST',
        data: {},
        url: '/NoteBook/GetSectionList',
        success: function (result) {
            var section = '<option value="0">All Section</option>';
            for (var i = 0; i < result.length; i++) {
                if (subspecilty > 0) {
                    if (subspecilty == result[i].SubSpecialtyId) {
                        section += '<option value="' + result[i].SubSpecialtyId + '" selected>Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                    }
                    else {
                        section += '<option value="' + result[i].SubSpecialtyId + '">Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                    }
                } else {
                    section += '<option value="' + result[i].SubSpecialtyId + '">Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                }

            }
            $('#SectionList').html(section);
            $(".mylist").selectBoxIt({
                theme: "default",
                autoWidth: false
            });
            LoadChapter(subspecilty, chapterName);
        }
    });

    $('#SectionList').on('change', function () {
        LoadChapter(sectionid = 0, chapterName = null);
    });


    function LoadChapter(sectionid, chapterName) {
        var specialtyId = 0;
        if (sectionid > 0) {
            specialtyId = sectionid;
        } else {
            specialtyId = $('#SectionList option:selected').val();
        }
        specialtyId > 0 ? $('#ChapterList').removeAttr('disabled') : $('#ChapterList').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            data: { subSpecialtyId: specialtyId },
            url: '/NoteBook/GetChapterList',
            success: function (result) {
                var chapter = '<option value="0">All Chapters</option>';
                for (var i = 0; i < result.length; i++) {
                    if (chapterName != '') {
                        if (chapterName == result[i].ChapterName) {
                            chapter += '<option value="' + result[i].ChapterName + '"selected>' + result[i].ChapterName + '</option>';
                        }
                        else {
                            chapter += '<option value="' + result[i].ChapterName + '">' + result[i].ChapterName + '</option>';
                        }
                    } else {
                        chapter += '<option value="' + result[i].ChapterName + '">' + result[i].ChapterName + '</option>';
                    }
                }
                $(".mylist1").selectBoxIt({
                    theme: "default",
                    autoWidth: false
                });
                $('#ChapterList').html(chapter);
                //$(".mylist1").selectBoxIt().refresh();
                //var container = $('.tet');
                //container.find('select').data('selectBox-selectBoxIt').destroy();
                //container.remove();
                //$('.mylist1').data('selectBox-selectBoxIt').refresh();
                $('.mylist1').data("selectBox-selectBoxIt").refresh();
            }
        });
    }

    function pagination(PageNoVal) {
        var totalnoofitem = NotebookViewModel.chapterCount;
        var noOfRecords = NotebookViewModel.noOfRecords;
        //$("#ChapterView").val(noOfRecords).change();
        var limitperpage = 5; //$('#ChapterView option:selected').val();
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
            if (i == PageNoVal) {
                $(".pagination").append("<li  class='currentpage active' id='" + i + "Page'><a href='javascript:void(0)'>" + i + "</a></li>");
            }
            else {
                if (i <= 5) {
                    $(".pagination").append("<li class='currentpage' id='" + i + "Page'><a href='javascript:void(0)'>" + i + "</a></li>");
                }
                else {
                    $(".pagination").append("<li class='currentpage hidden'><a href='javascript:void(0)'>" + i + "</a></li>");
                }

            }
        }

        $(".pagination").append("<li id='Nextpage'> <a  href='javascript:void(0)'>Next</a></li>");
        $(".pagination").append("<li id='Lastpage'> <a id='Last' href='javascript:void(0)' TotalPages='" + totalpages + "'>Last</a></li>");

        if (totalpages <= 10) {
            $("#Lastpage").hide();
            $("#Firstpage").hide();
        }

        if (totalpages <= 1) {
            $("#Nextpage").hide();
            $("#Previouspage").hide();
        }

        $(".pagination li.currentpage").on("click", function () {
            if ($(this).hasClass("active")) {
                return false;
            }
            else {
                var currentpage = $(this).index();
                currentpage = (currentpage - 1);
                var getdata = GetChapterQuestions(currentpage);
                $(".pagination li").removeClass("active");
                $(this).addClass("active");
            }

        });

        $("#Nextpage").on("click", function () {
            var currentpage = $(".pagination li.active").index();
            if (currentpage > totalpages) {
                return false;
            }
            else {
                GetChapterQuestions(currentpage);
            }
        });
        $("#Previouspage").on("click", function () {
            var currentpage = $(".pagination li.active").index();
            if (currentpage === 2) {
                return false;
            } else if (currentpage >= 1) {
                currentpage = currentpage - 2;
                GetChapterQuestions(currentpage);
            }
        });
    }

    $('#GetQuestionDetails').on('click', function () {
        pageIndex = 1;
        GetChapterQuestions(pageIndex);
    });

    function GetChapterQuestions(pageIndex) {
        $("#DivIdToPrint").hide();
        var noOfRecords = 5; //$('#ChapterView option:selected').val();
        var SubSpecialtyId = $('#SectionList option:selected').val();
        var ChapterName = $('#ChapterList option:selected').val();
        var Chapter = $('#ChapterList option:selected').val();
        window.location.href = "/NoteBook/GetQuestions?subSpecialtyId=" + SubSpecialtyId + "&chapterName=" + ChapterName + "&noOfRecords=" + noOfRecords + "&pageNo=" + pageIndex
    }
    //function GetChapterQuestions(pageIndex) {
    //    var noOfRecords = 5; //$('#ChapterView option:selected').val();
    //    var SubSpecialtyId = $('#SectionList option:selected').val();

    //    var ChapterName = $('#ChapterList option:selected').val();
    //   window.location.href = "/NoteBook/GetQuestions?subSpecialtyId=" + SubSpecialtyId + "&chapterName=" + ChapterName + "&noOfRecords=" + noOfRecords + "&pageNo=" + pageIndex
    //      }

    /* Add Notes Start*/
    $('.createNewNoteClass').on('click', function () {
        $("#NoteTextArea").val('');
        tinymce.init({
            selector: 'textarea#NoteTextArea',
            menubar: false,
            branding: false,
            height: 200,
            forced_root_block: false,
            plugins: [
            'advlist autolink lists link image charmap print preview anchor textcolor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media table paste code help wordcount'
            ],
            toolbar: 'bold italic underline backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            //toolbar: 'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            content_css: [
            '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
            '//www.tiny.cloud/css/codepen.min.css'
            ]
        });
        $("#createNotes").modal('show');
        var questionId = $(this).attr('QuestionId');
        var chapterCounter = $(this).attr('ChapterCounter');
        $(".modal-body #ChapterCounter").val(chapterCounter);
        $(".modal-body #createNotesCurrentQuestionId").val(questionId);
        tinymce.get('NoteTextArea').setContent('');
        var subType = null;
    });

    $(document).on('click', '#AddNoteBtn', function (event) {
        var trimNotesStr = tinymce.get("NoteTextArea").getContent();
        var QuestionId = $("#createNotesCurrentQuestionId").val();
        var ChapterCounter = $("#ChapterCounter").val();
        if (trimNotesStr == "") {
            var ErrNotesmsg = "Please enter your notes"
            $(".Notespan").text(ErrNotesmsg);
            return false;
        }
        else {
            var NotesTextAreaDetails = $("#NoteTextArea").val();
            $.ajax({
                type: 'post',
                async: false,
                data: { questionId: QuestionId, notesText: encodeURIComponent(trimNotesStr) },
                url: '/NoteBook/AddNotes',
                contentType: "application/x-www-form-urlencoded",
                dataType: "json",
                success: function (result) {
                    $('#notes' + result[0].QuestionId).val('');
                    var NotesList = "";
                    NotesList += '<div><ul>'
                    for (var i = 0; i < result.length; i++) {

                        NotesList += '<li id="notes' + result[i].QuestionId + result[i].NotesId + '"><div class="martop10 EditText" id="edit-note-textarea-action' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" style="display: none;"><span class="Notespan error-text"></span><textarea id="edittextarea' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" class="EditNotesTextArea"></textarea><button class="btn btn-primary SaveEdit" id="EditNoteBtn' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Save</button><button class="btn btn-default CancelEdit" id="CancelEdit' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '"  questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Cancel</button></div><div class="martop10" id="textarea_show' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '"><span class="NotesContent">' + result[i].NotesText + '</span><a href="javascript:void(0)" id="EditNotes' + result[i].NotesId + '" class="EditNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="EditNotesIcon' + result[i].NotesId + '" class="fa fa-edit marleft10"></i></a><a href="javascript:void(0)" id="DeleteNotes' + result[i].NotesId + '" class="DeleteNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="DeleteNotesIcon' + result[i].NotesId + '" class="fa fa-trash marleft10"></i></a></div></li>'

                    }
                    NotesList += '</ul></div>'
                    $('#notes' + result[0].QuestionId + ChapterCounter).html(NotesList);
                }
            });
        }
        $("#NoteTextArea").val('');
    });


    /*Add Notes End*/

    /**Edit notes click*/
    $(document).on('click', '.EditNoteClass', function () {
        var NotesId = $(this).attr('NotesId');
        var QuestionId = $(this).attr('QuestionId');
        var ChapterCounter = $(this).attr('ChapterCounter');
        var NotesText = $(this).parent().parent().find(".NotesContent").html();

        $("#textarea_show" + QuestionId + NotesId + ChapterCounter).hide();
        $("#edit-note-textarea-action" + QuestionId + NotesId + ChapterCounter).show();
        var textarea1 = "#" + $(this).parent().parent().find('#edittextarea' + QuestionId + NotesId + ChapterCounter).attr('id');
        $(textarea1).val(NotesText);
        tinymce.init({
            selector: textarea1,
            height: 100,
            menubar: false,
            branding: false,
            statusbar: false,
            autoresize_on_init: false,
            forced_root_block: false,
            plugins: [
              'advlist autolink lists link image charmap print preview anchor textcolor',
              'searchreplace visualblocks code fullscreen',
              'insertdatetime media table paste code help wordcount'
            ],
            //toolbar: 'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            toolbar: 'bold italic underline backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
            content_css: [
              '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
              '//www.tiny.cloud/css/codepen.min.css'
            ]
        });

        $(this).parent().parent().addClass("edit-note-textarea");
        var subType = null;
    });
    $(document).on('click', '.CancelEdit', function (event) {
        var NotesId = $(this).attr('NotesId');
        var NotesText = $(this).attr('NotesText');
        var QuestionId = $(this).attr('QuestionId');
        var ChapterCounter = $(this).attr('ChapterCounter');
        tinymce.get('edittextarea' + QuestionId + NotesId + ChapterCounter).remove();
        $("#edit-note-textarea-action" + QuestionId + NotesId + ChapterCounter).hide();
        $("#textarea_show" + QuestionId + NotesId + ChapterCounter).show();

        $(this).parent().parent().find(".NotesContent").show();
        $(this).parent().parent().removeClass("edit-note-textarea");
    });
    $(document).on('click', '.SaveEdit', function () {
        var NotesId = $(this).attr('NotesId');
        var QuestionId = $(this).attr('QuestionId');
        var ChapterCounter = $(this).attr('ChapterCounter');
        var trimNotesStr = tinymce.get('edittextarea' + QuestionId + NotesId + ChapterCounter).getContent();

        if (trimNotesStr == "") {
            var ErrNotesmsg = "Please enter your notes"
            $(".Notespan").text(ErrNotesmsg);
            return false;
        }
        else {
            $.ajax({
                type: 'post',
                data: { notesId: NotesId, questionId: QuestionId, notesText: encodeURIComponent(trimNotesStr) },
                url: '/NoteBook/UpdateNotes',
                // dataType:'html',
                success: function (result) {
                    $('#notes' + result[0].QuestionId + ChapterCounter).empty();
                    $('#notes' + result[0].QuestionId + ChapterCounter).val("");
                    var NotesList = "";
                    NotesList += '<div><ul>'
                    for (var i = 0; i < result.length; i++) {
                        NotesList += '<li id="notes' + result[i].QuestionId + result[i].NotesId + '"><div class="martop10 EditText" id="edit-note-textarea-action' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" style="display: none;"><span class="Notespan error-text"></span><textarea id="edittextarea' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" class="EditNotesTextArea"></textarea><button class="btn btn-primary SaveEdit" id="EditNoteBtn' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Save</button><button class="btn btn-default CancelEdit" id="CancelEdit' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '"  questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Cancel</button></div><div class="martop10" id="textarea_show' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '"><span class="NotesContent">' + result[i].NotesText + '</span><a href="javascript:void(0)" id="EditNotes' + result[i].NotesId + '" class="EditNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="EditNotesIcon' + result[i].NotesId + '" class="fa fa-edit marleft10"></i></a><a href="javascript:void(0)" id="DeleteNotes' + result[i].NotesId + '" class="DeleteNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="DeleteNotesIcon' + result[i].NotesId + '" class="fa fa-trash marleft10"></i></a></div></li>'
                        // NotesList += '<li id="notes' + result[i].QuestionId + result[i].NotesId + '"><div class="martop10 EditText" id="edit-note-textarea-action' + result[i].QuestionId + result[i].NotesId + ChapterCounter +  '" style="display: none;"><span class="Notespan error-text"></span><textarea id="edittextarea' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" class="EditNotesTextArea"></textarea><button class="btn btn-primary SaveEdit" id="EditNoteBtn' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '" notestext="' + result[i].NotesText + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Save</button><button class="btn btn-default CancelEdit" id="CancelEdit' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '" notestext="' + result[i].NotesText + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Cancel</button></div><div class="martop10" id="textarea_show' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '"><span class="NotesContent">' + result[i].NotesText + '</span><a href="javascript:void(0)" id="EditNotes' + result[i].NotesId + '" class="EditNoteClass NotesContent" notesid="' + result[i].NotesId + '" notestext="' + result[i].NotesText + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="EditNotesIcon' + result[i].NotesId + '" class="fa fa-edit marleft10"></i></a><a href="javascript:void(0)" id="DeleteNotes' + result[i].NotesId + '" class="DeleteNoteClass NotesContent" notesid="' + result[i].NotesId + '" notestext="' + result[i].NotesText + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="DeleteNotesIcon' + result[i].NotesId + '" class="fa fa-trash marleft10"></i></a></div></li>'
                    }
                    NotesList += '</ul></div>'
                    $('#notes' + result[0].QuestionId + ChapterCounter).html(NotesList);
                    tinymce.get('edittextarea' + QuestionId + NotesId + ChapterCounter).remove();

                    $(this).parent().parent().find(".NotesContent").show();
                    $(this).parent().parent().removeClass("edit-note-textarea");
                }
            });

            $("#textarea_show" + QuestionId + NotesId + ChapterCounter).show();
            $("#edit-note-textarea-action" + QuestionId + NotesId + ChapterCounter).hide();
        }
    });

    /***Edit notes click end****/

    /***Delete notes click Start****/
    $(document).on('click', '.DeleteNoteClass', function () {
        $("#deleteNotes").modal('show');
        var NotesId = $(this).attr('NotesId');
        var QuestionId = $(this).attr('QuestionId');
        var chapterCounter = $(this).attr('ChapterCounter');
        $(".modal-body #ChapterCounter").val(chapterCounter);
        $(".modal-body #deleteNotesId").val(NotesId);
        $(".modal-body #deleteNotesQuestionId").val(QuestionId);
        var subType = null;
    });

    $(document).on('click', '#DeleteNoteBtn', function (event) {
        var NotesId = $("#deleteNotesId").val();
        var QuestionId = $("#deleteNotesQuestionId").val();
        var ChapterCounter = $("#ChapterCounter").val();
        $.ajax({
            type: 'post',
            data: { notesId: NotesId, questionId: QuestionId },
            url: '/NoteBook/DeleteNotes',
            success: function (result) {
                $('#notes' + result[0].QuestionId).val('');
                var NotesList = "";
                NotesList += '<div><ul>'
                for (var i = 0; i < result.length; i++) {
                    if (result[i].NotesId != 0) {
                        NotesList += '<li id="notes' + result[i].QuestionId + result[i].NotesId + '"><div class="martop10 EditText" id="edit-note-textarea-action' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" style="display: none;"><span class="Notespan error-text"></span><textarea id="edittextarea' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" class="EditNotesTextArea"></textarea><button class="btn btn-primary SaveEdit" id="EditNoteBtn' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Save</button><button class="btn btn-default CancelEdit" id="CancelEdit' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '" notesid="' + result[i].NotesId + '"  questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '">Cancel</button></div><div class="martop10" id="textarea_show' + result[i].QuestionId + result[i].NotesId + ChapterCounter + '"><span class="NotesContent">' + result[i].NotesText + '</span><a href="javascript:void(0)" id="EditNotes' + result[i].NotesId + '" class="EditNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="EditNotesIcon' + result[i].NotesId + '" class="fa fa-edit marleft10"></i></a><a href="javascript:void(0)" id="DeleteNotes' + result[i].NotesId + '" class="DeleteNoteClass NotesContent" notesid="' + result[i].NotesId + '" questionid="' + result[i].QuestionId + '" chaptercounter="' + ChapterCounter + '"><i id="DeleteNotesIcon' + result[i].NotesId + '" class="fa fa-trash marleft10"></i></a></div></li>'
                    } else {
                        window.location.reload(true);
                        return true;
                    }
                }
                NotesList += '</ul></div>'
                $('#notes' + result[0].QuestionId + ChapterCounter).html(NotesList);
            }
        });
    });

    /***Delete notes click end****/
    $('a.printPage').click(function (e) {

        e.preventDefault();
        e.stopPropagation();

        var SubSpecialtyId = $('#SectionList option:selected').val();
        var ChapterName = $('#ChapterList option:selected').val();

        $.ajax({
            type: 'POST',
            data: { chapterName: ChapterName, subSpecialtyId: SubSpecialtyId },
            url: '/NoteBook/GetNotesList',
            success: function (result) {
            }
        });
        var w = 200;
        var h = 200;
        var left = Number((screen.width / 2) - (w / 2));
        var tops = Number((screen.height / 2) - (h / 2));
        var url = "/NoteBook/GetNotesList?chapterName=" + chapterName + "&SubSpecialtyId=" + SubSpecialtyId;
        // window.open(url, "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=" + screen.availWidth + ",height=" + screen.availHeight)
        window.open(url, "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=100,left=100,width=700,height=700")
        return false;
    });

    $(".notes-panel .panel-collapse").on("hide.bs.collapse", function () {
        $(this).parent().find("#quesText").removeClass("panel-open");
        $(this).parent().find("#quesText .question-number-view-exam-details i").removeClass("fa-angle-up").addClass("fa-angle-down");
    });
    $(".notes-panel .panel-collapse").on("show.bs.collapse", function () {
        $(this).parent().find("#quesText").addClass("panel-open");
        $(this).parent().find("#quesText .question-number-view-exam-details i").removeClass("fa-angle-down").addClass("fa-angle-up");
    });
});