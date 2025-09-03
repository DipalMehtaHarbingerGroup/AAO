namespace AAO.DTO.BCSCSelfAssessment
{
    using System;

    public class ResidentChapterReportDetailsDTO
    {
            public int UserId { get; set; }

            public int SubspecialtyId { get; set; }

            public string Subspecialty { get; set; }

            public string ChapterName { get; set; }

            public int Correct { get; set; }

            public int InCorrect { get; set; }

            public int RemainingQuestion { get; set; }

            public DateTime? ExamStartDate { get; set; }

            public int BCSCSectionNumber { get; set; }

            public DateTime? ExamCompletedDate { get; set; }

            public int Year { get; set; }
    }
}
