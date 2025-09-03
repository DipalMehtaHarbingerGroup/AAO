$(document).ready(function () {

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
});