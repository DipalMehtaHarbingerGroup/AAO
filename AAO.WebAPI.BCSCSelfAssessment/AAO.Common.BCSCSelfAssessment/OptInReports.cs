namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class OptInReports
    {
        public string PGYYear { get; set; }

        public int Year { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public object Seq { get; set; }

        public object OrderBy { get; set; }

        public int TotalUserCount { get; set; }

        public string MasterCustomerID { get; set; }

        public string OptIn { get; set; }

        public int TotalOptInAcceptCount { get; set; }

        public int TotalOptInDeclineCount { get; set; }
    }
}
