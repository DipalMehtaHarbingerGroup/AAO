namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public static class ResidentChapterReportDetailsBL
    {
        public static List<ResidentChapterReportDetailsDTO> ResidentChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsDAL.ResidentChapterReportDetails(reportChapterDetails);
        }

        public static List<ResidentChapterReportDetailsDTO> ResidentChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return ResidentChapterReportDetailsDAL.ResidentChapterReportDetails(reportChapterDetails);
        }
    }
}
