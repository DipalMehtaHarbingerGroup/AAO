//if (QuestionViewModel.examTimeType == 1) {
//    $(document).ready(function () {
//        $.ajax({
//            type: "POST",
//            dataType: "json",
//            data: { examId: QuestionViewModel.examId },
//            url: "/Assessment/GetExamTime",
//            success: function (response) {
//                if (QuestionViewModel.examStatus != 2 && QuestionViewModel.examTimeType == 1) {
//                    var dt = response;
//                    var dateAr = dt.split('-');
//                    var newDate = dateAr[1] + '/' + dateAr[0] + '/' + dateAr[2];
//                    var end = new Date(dateAr[1] + '/' + dateAr[0] + '/' + dateAr[2]);
//                    var _second = 1000;
//                    var _minute = _second * 60;
//                    var _hour = _minute * 60;
//                    var _day = _hour * 24;
//                    var timer;
//                    function showRemaining() {
//                        var now = new Date();
//                        var remainingTime = end - now;
//                        if (remainingTime < 0) {
//                            clearInterval(timer);
//                            document.getElementById('timecountdown').innerHTML = 'Count Down Ends!';
//                            $.ajax({
//                                url: QuestionViewModel.url,
//                                type: 'POST',
//                                cache: false,
//                                dataType: "html",
//                                data: { ExamId: QuestionViewModel.examId, ExamattemptId: QuestionViewModel.ExamattemptId },
//                                success: function (result) {
//                                    if (result == 1) {
//                                        window.location.href = '/ExamHistory/InfoMessage';
//                                    }
//                                }
//                            });
//                        }

//                        var days = Math.floor(remainingTime / _day);
//                        var _hours = Math.floor((remainingTime % _day) / _hour);
//                        var _minutes = Math.floor((remainingTime % _hour) / _minute);
//                        var _seconds = Math.floor((remainingTime % _minute) / _second);
//                        if (_hours > 0) {
//                            document.getElementById('timecountdown').innerHTML = '' +('0' + _hours).slice(-2);
//                            document.getElementById('timecountdown').innerHTML += ':' + ('0' + _minutes).slice(-2);
//                        } else {
//                            document.getElementById('timecountdown').innerHTML = '';
//                            document.getElementById('timecountdown').innerHTML += ''+('0' + _minutes).slice(-2);
//                        }
//                        document.getElementById('timecountdown').innerHTML += ':' + ('0' + _seconds).slice(-2);
//                    }
//                    timer = setInterval(showRemaining, 0);
//                    $('#icontoshow').show();
//                }
//                else {
//                    $('#timecountdown').show();
//                    $('#icontoshow').hide();
//                }
//            },

//        });
//    });
//}


