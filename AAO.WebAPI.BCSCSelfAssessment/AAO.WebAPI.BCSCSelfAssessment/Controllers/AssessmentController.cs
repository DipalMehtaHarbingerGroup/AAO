namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class AssessmentController : ApiController
    {
        [Route("api/Assessment/GetExamQuestion")]
        [HttpPost]
        public QuestionDetails GetExamQuestion(UpdateSkipAnswered question)
        {
            return AssessmentBL.GetExamQuestion(question.examId, question.questionId, question.userId);
        }

        // [Route("api/Assessment/UpdateExamAttempt")]
        // [HttpPost]
        // public QuestionDetails UpdateExamAttempt(QuestionDetails question)
        // {
        //    return AssessmentBL.UpdateExamAttempt(question.examId, question.userId, question.examStatus);
        // }
        [Route("api/Assessment/UpdateSkipAnswerQuestion")]
        [HttpPost]
        public int UpdateSkipAnswerQuestion(UpdateSkipAnswered updateAnswered)
        {
            return AssessmentBL.UpdateSkipAnswerQuestion(updateAnswered);
        }

        [Route("api/Assessment/AddNotesAnswerText")]
        [HttpPost]
        public void AddNotesAnswerText(UpdateSkipAnswered notesAnswer)
        {
            AssessmentBL.AddNotesAnswerText(notesAnswer);
        }

        [Route("api/Assessment/MarkQuestionCheck")]
        [HttpPost]
        public bool MarkQuestionCheck(UpdateSkipAnswered updateMark)
        {
            return AssessmentBL.MarkQuestionCheck(updateMark);
        }

        [Route("api/Assessment/GetExamDetail")]
        [HttpPost]
        public ExamDTO GetExamDetail(UpdateSkipAnswered exam)
        {
            return AssessmentBL.GetExamBYId(exam.examId);
        }

        [Route("api/Assessment/SetTimeForQuestion")]
        [HttpPost]
        public int SetTimeForQuestion(ExamDTO exam)
        {
            return AssessmentBL.SetTimeForQuestion(exam.ExamAttemptId);
        }

        [Route("api/Assessment/UpdateConsumedTime")]
        [HttpPost]
        public void UpdateConsumedTime(UpdateSkipAnswered updateTime)
        {
            AssessmentBL.UpdateConsumedTime(updateTime);
        }

        [Route("api/Assessment/SubmitExam")]
        [HttpPost]
        public int SubmitExam(UpdateSkipAnswered submitExam)
        {
            return AssessmentBL.SubmitExam(submitExam);
        }

        [Route("api/Assessment/ViewProgress")]
        [HttpPost]
        public List<ViewProgressVM> ViewProgress(UpdateSkipAnswered viewProgress)
        {
            return AssessmentBL.ViewProgress(viewProgress);
        }

        [Route("api/Assessment/SaveIsYourAnswer")]
        [HttpPost]
        public void SaveIsYourAnswer(UpdateSkipAnswered isYourAnswer)
        {
            AssessmentBL.SaveIsYourAnswer(isYourAnswer);
        }

        [Route("api/Assessment/GetRightChoiceId")]
        [HttpPost]
        public int GetRightChoiceId(RightQuestionIdVM questionId)
        {
            return AssessmentBL.GetRightChoiceId(questionId);
        }

        [Route("api/Assessment/UpdateTime")]
        [HttpPost]
        public void UpdateTime(UpdateSkipAnswered updateTime)
        {
            AssessmentBL.UpdateTime(updateTime);
        }

        [Route("api/Assessment/GetNotes")]
        [HttpPost]
        public List<string> GetNotes(UpdateSkipAnswered getNotes)
        {
            return AssessmentBL.GetNotes(getNotes);
        }

        [Route("api/Assessment/GetExcerpt")]
        [HttpPost]
        public QuestionDetails GetExcerpt(QuestionDetails questions)
        {
            return AssessmentBL.GetExcerpt(questions.examId, questions.questionId, questions.userId);
        }
    }
}
