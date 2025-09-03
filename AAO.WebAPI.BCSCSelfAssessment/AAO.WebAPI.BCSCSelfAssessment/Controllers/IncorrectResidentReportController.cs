namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using Common.BCSCSelfAssessment;

    public class IncorrectResidentReportController : ApiController
    {
        [Route("api/IncorrectResidentReport/IncorrectQuestionDetails")]
        [HttpPost]
        public List<QuestionDetails> IncorrectQuestionDetails(ReportsVM incorrectReportDetails)
        {
            return IncorrectQuestionDetailsBL.IncorrectQuestionDetails(incorrectReportDetails);
        }
    }
}
