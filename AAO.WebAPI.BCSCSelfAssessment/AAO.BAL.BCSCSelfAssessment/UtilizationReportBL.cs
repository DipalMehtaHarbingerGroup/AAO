namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using Common.BCSCSelfAssessment;
    using DAL.BCSCSelfAssessment;

    public class UtilizationReportBL
    {
        public static List<UtilizationReportsVM> UtilizationReports(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportsDAL.UtilizationReports(utilizationReports);
        }

        public static UtilizationReportsVM AtAGlance(UtilizationReportsVM utilizationReports)
        {
            return UtilizationReportsDAL.AtAGlance(utilizationReports);
        }
    }
}
