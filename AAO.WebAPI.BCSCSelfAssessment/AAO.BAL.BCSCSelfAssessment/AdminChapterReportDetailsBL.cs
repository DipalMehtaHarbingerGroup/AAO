namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public class AdminChapterReportDetailsBL
    {
        public static List<ResidentChapterReportDetailsDTO> AdminChapterReportDetails(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsDAL.AdminChapterReportDetails(reportChapterDetails);
        }

        public static List<ResidentChapterReportDetailsDTO> AdminChartDetail(ResidentChapterReportDetailsDTO reportChapterDetails)
        {
            return AdminChapterReportDetailsDAL.AdminChapterReportDetails(reportChapterDetails);
        }
    }
}
