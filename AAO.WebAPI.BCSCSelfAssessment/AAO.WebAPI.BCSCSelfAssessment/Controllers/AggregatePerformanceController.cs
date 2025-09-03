namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;

    public class AggregatePerformanceController : ApiController
    {
        [Route("api/AggregatePerformance/ReportDetails")]
        [HttpPost]
        public List<ReportsDetailsVM> ReportDetails(ReportsDetailsVM reportDetails)
        {
            return ReportDetailsBL.ReportDetails(reportDetails);
        }
    }
}
