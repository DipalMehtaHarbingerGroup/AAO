namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using BAL.BCSCSelfAssessment;
    using Common.BCSCSelfAssessment;
    using System.Collections.Generic;
    using System.Web.Http;

    public class UtilizationReportsController : ApiController
    {
        [Route("api/UtilizationReports/UtilizationReports")]
        [HttpPost]
        public List<UtilizationReportsVM> UtilizationReports(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportBL.UtilizationReports(utilizationReports);
        }

        [Route("api/UtilizationReports/AtAGlance")]
        [HttpPost]
        public UtilizationReportsVM AtAGlance(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportBL.AtAGlance(utilizationReports);
        }
    }
}
