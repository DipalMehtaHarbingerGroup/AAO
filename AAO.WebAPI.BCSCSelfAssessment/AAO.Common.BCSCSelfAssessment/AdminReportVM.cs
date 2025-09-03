namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class AdminReportVM
    {
        public int SubspecialtyId { get; set; }

        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public string SubspecialtyName { get; set; }

        public int UserId { get; set; }

        public int Correct { get; set; }

        public int InCorrect { get; set; }

        public int Score { get; set; }

        public int? NoOfRecords { get; set; }

        public int? PageNo { get; set; }

        public int BCSCSectionNumber { get; set; }

        public string PGYYear { get; set; }

        public int Year { get; set; }

        public object Seq { get; set; }

        public object OrderBy { get; set; }
    }
}
