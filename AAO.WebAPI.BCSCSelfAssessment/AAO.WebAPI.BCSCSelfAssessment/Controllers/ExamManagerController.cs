namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
    using AAO.WebAPI.BCSCSelfAssessment.ExceptionFilter;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Http;

    public class ExamManagerController : ApiController
    {
        [CustomExceptionFilter]
        [Route("api/ExamManager/GetSpecialityList")]
        [HttpPost]
        public List<SubSpecialityDetailVM> GetSpecialityList(ExamDTO exam)
        {
            return SpecialityBL.GetSpecialityList(exam);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/CreateExam")]
        [HttpPost]
        public ResponseStatusVM CreateExam(ExamDTO objCreateExam)
        {
            return ExamBL.CreateExam(objCreateExam);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/GetExamById")]
        [HttpGet]
        public ExamDTO GetExamById(int? examId)
        {
            return ExamBL.GetExamBYId(examId);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/GetQuestionTypeCount")]
        [HttpPost]
        [HttpGet]
        public List<QuestionTypeCountDTO> GetQuestionTypeCount(ExamDTO ex)
        {
            return ExamBL.GetQuestionTypeCount(ex.UserId);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/GetExamIdBYUserId")]
        [HttpGet]
        [HttpPost]
        public int GetExamIdBYUserId(ExamDTO examDto)
        {
            return ExamBL.GetExamIdBYUserId(examDto.UserId);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/CheckExamNameAvailable")]
        [HttpGet]
        [HttpPost]
        public string CheckExamNameAvailable(ExamNameVM examName)
        {
            return ExamBL.CheckExamNameAvailable(examName);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/GetExamCountOnExamType")]
        [HttpGet]
        [HttpPost]
        public ExamCountOnExamTypeVM GetExamCountOnExamType(ExamCountOnExamTypeVM examCount)
        {
            return ExamBL.GetExamCountOnExamType(examCount.UserId);
        }

        [CustomExceptionFilter]
        [Route("api/ExamManager/GetQuestionTypeCountBySection")]
        [HttpGet]
        [HttpPost]
        public QuestionCountOnSection GetQuestionTypeCountBySection(QuestionCountOnSection sectionValue)
        {
            return ExamBL.GetQuestionTypeCountBySection(sectionValue);
        }

        [Route("api/ExamManager/SearchEmails")]
        [HttpPost]
        public List<EmailSuggestionDTO> SearchEmails(EmailSearchDTO searchRequest)
        {
            return ExamBL.SearchEmails(searchRequest);
        }
    }
}