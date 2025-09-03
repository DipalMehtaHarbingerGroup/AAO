namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AdminQuestionPerformanceController : ApiController
    {
        [Route("api/AdminQuestionPerformance/AdminQuestionPerformanceDetails")]
        [HttpPost]
        public List<AdminQuestionPerformanceVM> AdminQuestionPerformanceDetails(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceBL.AdminQuestionPerformanceDetails(queDetails);
        }

        [Route("api/AdminQuestionPerformance/GetAllQuestionCount")]
        [HttpPost]
        public int GetAllQuestionCount(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceBL.GetAllQuestionCount(queDetails);
        }

        [Route("api/AdminQuestionPerformance/GetAllQuestionExport")]
        [HttpPost]
        public List<AdminQuestionPerformanceVM> GetAllQuestionExport(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceBL.GetAllQuestionExport(queDetails);
        }

        [Route("api/AdminQuestionPerformance/GetAllAnsweredQuestionExport")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetAllAnsweredQuestionExport([FromBody]string content)
        {
            List<AdminExamQuestionReport> result = new List<AdminExamQuestionReport>();
            AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM();
            queDetails = JsonConvert.DeserializeObject<AdminQuestionPerformanceVM>(content);
            result = await AdminQuestionPerformanceBL.GetAllAnsweredQuestionExport(queDetails);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
           // Response responseclass = new Response(true, "SUCCESS", result);
           // return responseclass;
            //return Content(HttpStatusCode.OK, responseclass, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/AdminQuestionPerformance/GetAllAnsweredQuestionExport1")]
        [HttpPost]
        public List<AdminExamQuestionReport> GetAllAnsweredQuestionExport1(AdminQuestionPerformanceVM content)
        {
            List<AdminExamQuestionReport> res = new List<AdminExamQuestionReport>();
           // AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM();
           // queDetails = JsonConvert.DeserializeObject<AdminQuestionPerformanceVM>(content);
            res= AdminQuestionPerformanceBL.GetAllAnsweredQuestionExport1(content);
            return res;
        }

        [Route("api/AdminQuestionPerformance/GetTableCountExport")]
        [HttpPost]
        public int GetTableCountExport(AdminQuestionPerformanceVM content)
        {
            return AdminQuestionPerformanceBL.GetTableCountExport(content);
        }
    }
}