namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class AdminChapterReportController : ApiController
    {
        [Route("api/AdminChapterReport/AdminChapterReportDetails")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> AdminChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsBL.AdminChapterReportDetails(reportChapterDetails);
        }

        [Route("api/AdminChapterReport/AdminChartDetail")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> AdminChartReportDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsBL.AdminChartDetail(reportChapterDetails);
        }
    }
}
