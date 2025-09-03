namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class ResidentChapterReportController : ApiController
    {
        [Route("api/ResidentChapterReport/ResidentChapterReportDetails")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> ResidentChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsBL.ResidentChapterReportDetails(reportChapterDetails);
        }

        [Route("api/ResidentChapterReport/ResidentChartDetail")]
        [HttpPost]
        public List<ResidentChapterReportDetailsDTO> ResidentChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsBL.ResidentChartDetail(reportChapterDetails);
        }
    }
}
