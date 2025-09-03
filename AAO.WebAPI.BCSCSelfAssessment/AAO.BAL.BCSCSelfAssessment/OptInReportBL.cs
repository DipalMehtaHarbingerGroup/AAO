namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using Common.BCSCSelfAssessment;
    using DAL.BCSCSelfAssessment;

    public class OptInReportBL
    {
        public static List<OptInReports> OptInReports(OptInReports optInReports)
        {
            return OptInReportsDAL.OptInReports(optInReports);
        }

        public static List<OptInReports> GetOptIn(OptInReports optInReports)
        {
            return OptInReportsDAL.GetOptIn(optInReports);
        }
    }
}
