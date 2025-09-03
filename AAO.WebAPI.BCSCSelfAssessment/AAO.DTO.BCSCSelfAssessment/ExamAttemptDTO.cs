namespace AAO.DTO.BCSCSelfAssessment
{
    using System;

    public class ExamAttemptDTO
    {
        public int ExamAttemptId { get; set; }

        public DateTime ExamCreatedDate { get; set; }

        public DateTime ExamLastAttemptDate { get; set; }

        public int ExamId { get; set; }

        public int UserId { get; set; }

        public bool ExamStatus { get; set; }

        public decimal TotalAllocatedTime { get; set; }

        public decimal TotalConsumedTime { get; set; }
    }
}
