namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class ReportsVM
    {
        public int? NoOfRecords { get; set; }

        public int? PageNo { get; set; }

        public int SubspecialtyId { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public int Year { get; set; }
    }
}
