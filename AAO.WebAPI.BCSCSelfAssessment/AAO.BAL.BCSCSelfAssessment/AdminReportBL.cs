namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using DAL.BCSCSelfAssessment;

    public static class AdminReportBL
    {
        public static List<AdminReportVM> AdminReportDetails(AdminReportVM adminreportDetails)
        {
            return AdminReportDAL.AdminReportDetails(adminreportDetails);
        }

        public static List<AdminReportVM> GetPYGYear(AdminReportVM pgyDetails)
        {
            return AdminReportDAL.GetPYGYear(pgyDetails);
        }

        public static List<AdminReportVM> AdminReportDetailsExport(AdminReportVM adminreportDetails)
        {
            return AdminReportDAL.AdminReportDetailsExport(adminreportDetails);
        }
    }
}
