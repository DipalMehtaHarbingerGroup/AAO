namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    public class DashboardChartVM
    {
        public int SubSpecialityId { get; set; }

        public string SubSpecialityName { get; set; }

        public int Correct { get; set; }

        public int Incorrect { get; set; }

        public int Remaining { get; set; }

        public int Total { get; set; }

        public int SelfScore { get; set; }

        public int PeerScore { get; set; }
    }

    public class ChartParameterVM
    {
        public int UserId { get; set; }

        public bool ScoreCompWithPeers { get; set; }

        public int ExamType { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class CMECreditVM
    {
        public int SubSpecialityId { get; set; }

        public int BCSCSectionNumber { get; set; }

        public string SubSpecialityName { get; set; }

        public int AttemptedCount { get; set; }

        public string CMECreditPath { get; set; }
    }

    public class UserIdVM
    {
        public int UserId { get; set; }
    }
}
