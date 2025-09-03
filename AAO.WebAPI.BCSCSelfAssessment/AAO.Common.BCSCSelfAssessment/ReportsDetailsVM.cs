namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class ReportsDetailsVM
    {
        public int UserId { get; set; }

        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public int SubspecialtyId { get; set; }

        public string SubspecialtyName { get; set; }

        public int Correct { get; set; }

        public int InCorrect { get; set; }

        public int BCSCSectionNumber { get; set; }

        public int Score { get; set; }
    }
}
