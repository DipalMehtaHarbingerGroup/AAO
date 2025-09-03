namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;

    public class IncorrectAdminReportController : ApiController
    {
        [Route("api/IncorrectAdminReport/AdminIncorrectQuestionDetails")]
        [HttpPost]
        public List<QuestionDetails> AdminIncorrectQuestionDetails(AdminReportVM incorrectReportDetails)
        {
            return AdminIncorrectQuestionDetailsBL.AdminIncorrectQuestionDetails(incorrectReportDetails);
        }
    }
}
