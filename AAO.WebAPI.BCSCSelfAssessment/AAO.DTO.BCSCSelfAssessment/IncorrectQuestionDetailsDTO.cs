namespace AAO.DTO.BCSCSelfAssessment
{
    using System;

    public class IncorrectQuestionDetailsDTO
    {
        public int SubspecialtyId { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public int ExamId { get; set; }

        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public int NoOfRecords { get; set; }

        public int PageNo { get; set; }

        public int QuestionIdCount { get; set; }

        public string Subspecialty { get; set; }

        public int QuestionNo { get; set; }

        public int BCSCSectionNumber { get; set; }

        public int QuestionCount { get; set; }

        public int Section { get; set; }

        public int Rownumber { get; set; }

        public int Year { get; set; }

        public int ExamType { get; set; }
    }
}
