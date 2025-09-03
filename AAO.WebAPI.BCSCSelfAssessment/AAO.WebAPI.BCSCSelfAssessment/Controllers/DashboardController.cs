namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.Common.BCSCSelfAssessment;
    using BAL.BCSCSelfAssessment;
    using DTO.BCSCSelfAssessment;

    // [RoutePrefix("api/login")]
    public class DashboardController : ApiController
    {
        [Route("api/dashboard/GetmyExamHistoryList")]
        [HttpPost]
        [HttpGet]
        public List<ExamHistoryVM> GetmyExamHistoryList(ServiceCallVM objExamHistory)
        {
            return ExamBL.ExamMyProgressView(objExamHistory.userId);
        }

        // Get Exam Status Count
        [Route("api/dashboard/GetExamStatusCount")]
        [HttpPost]
        public ExamStatusCountVM GetExamStatusCount(ServiceCallVM examService)
        {
            return ExamBL.GetExamStatusCount(examService);
        }

        // Get Exam Score
        [Route("api/dashboard/GetExamScore")]
        [HttpPost]
        public List<ExamScoreDetailVM> GetExamScore(ServiceCallVM examService)
        {
            return ExamBL.GetExamScore(examService);
        }

        // Get DashBoardChar Details
        [Route("api/dashboard/GetChartDetail")]
        [HttpPost]
        public List<DashboardChartVM> GetChartDetail(ChartParameterVM chartParameters)
        {
            return ExamBL.GetChartDetail(chartParameters);
        }

        // Get UserStatusIsFirst Details
        [Route("api/dashboard/GetUserStatusIsFirst")]
        [HttpPost]
        public int GetUserStatusIsFirst(ServiceCallVM userService)
        {
            return ExamBL.GetUserStatusIsFirst(userService);
        }

        // update UserStatusIsFirst Details
        [Route("api/dashboard/UpdateUserStatusIsFirst")]
        [HttpPost]
        public void UpdateUserStatusIsFirst(ServiceCallVM userService)
        {
             ExamBL.UpdateUserStatusIsFirst(userService);
        }

        [Route("api/dashboard/GetRenewal")]
        [HttpPost]
        public List<UserDataDTO> GetRenewal(ServiceCallVM userService)
        {
            return UserBL.GetRenewal(userService);
        }

        [Route("api/dashboard/GetUserOptIn")]
        [HttpPost]
        public string GetUserOptInStatus(ServiceCallVM userService)
        {
            return ExamBL.GetUserOptInStatus(userService);
        }

        [Route("api/dashboard/UpdateUserOptIn")]
        [HttpPost]
        public int UpdateUserOptIn(ServiceCallVM userService)
        {
            return ExamBL.UpdateUserOptIn(userService);
        }
    }
}
