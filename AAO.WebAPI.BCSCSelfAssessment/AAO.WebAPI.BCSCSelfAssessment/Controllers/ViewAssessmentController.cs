namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class ViewAssessmentController : ApiController
    {
        [Route("api/ViewAssessment/ViewAssessmentDetails")]
        [HttpPost]
        public List<QuestionDetails> ViewAssessmentDetails(ExamQuestionDTO examque)
        {
            return ViewAssessmentBL.ViewAssessmentDetails(examque);
        }

        [Route("api/ViewAssisment/FilterByQuestions")]
        [HttpPost]
        public List<QuestionDetails> FilterByQuestions(AssesmentDetailVM assDetail)
        {
            return ViewAssessmentBL.FilterByQuestions(assDetail);
        }

        [Route("api/ViewAssessment/ViewDetails")]
        [HttpPost]
        public List<QuestionDetails> ViewDetails(ExamQuestionDTO examque)
        {
            return ViewAssessmentBL.ViewDetails(examque);
        }

        [Route("api/ViewAssessment/SearchByQuestions")]
        [HttpPost]
        public List<QuestionDetails> SearchByQuestions(AssesmentDetailVM assDetail)
        {
            return ViewAssessmentBL.SearchByQuestions(assDetail);
        }

    }
}
