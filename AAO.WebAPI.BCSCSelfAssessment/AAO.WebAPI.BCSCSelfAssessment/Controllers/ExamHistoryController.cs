namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using DTO.BCSCSelfAssessment;

    public class ExamHistoryController : ApiController
    {
        [Route("api/ExamHistory/ExamHistoryDetails")]
        [HttpPost]
        public List<ExamHistoryDTO> ExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryBL.ExamHistoryDetails(examhistory);
        }

        [Route("api/ExamHistory/DeleteExamHistoryDetails")]
        [HttpPost]
        public void DeleteExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            ExamHistoryBL.DeleteExamHistoryDetails(examhistory);
        }

        [Route("api/ExamHistory/GetPdfDetails")]
        [HttpPost]
        public PdfDetailsDataVM GetPdfDetails(ExamHistoryDTO examhistory)
        {
          return ExamHistoryBL.GetPdfDetails(examhistory);
        }

        [Route("api/ExamHistory/ResetExam")]
        [HttpGet]
        [HttpPost]
        public int ResetExam(ExamCountOnExamTypeVM examCount)
        {
            return ExamHistoryBL.ResetExam(examCount.UserId);
        }

        [Route("api/ExamHistory/InsertorAddEmail_GetOptOutDetails")]
        [HttpGet]
        [HttpPost]
        public List<ExamHistoryDTO> InsertorAddEmail_GetOptOutDetails(ExamHistoryDTO values)
        {
            return ExamHistoryBL.InsertorAddEmail_GetOptOutDetails(values);
        }

        [Route("api/ExamHistory/OptOut")]
        [HttpGet]
        [HttpPost]
        public string OptOut(ExamQuestionDTO emailid)
        {
            return ExamHistoryBL.OptOut(emailid.Id);
        }
    }
}


