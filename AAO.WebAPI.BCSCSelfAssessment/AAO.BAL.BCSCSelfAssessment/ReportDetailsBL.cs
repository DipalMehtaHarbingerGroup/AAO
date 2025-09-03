namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;

    public static class ReportDetailsBL
    {
        public static List<ReportsDetailsVM> ReportDetails(ReportsDetailsVM reportDetails)
        {
            return ReportDetailsDAL.ReportDetails(reportDetails);
        }
    }
}
