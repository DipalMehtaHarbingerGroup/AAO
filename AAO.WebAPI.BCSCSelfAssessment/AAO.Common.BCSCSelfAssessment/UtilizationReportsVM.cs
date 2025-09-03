namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class UtilizationReportsVM
    {
        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public string PGYYear { get; set; }

        public int Year { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public int QuestionAnswered { get; set; }

        public object Seq { get; set; }

        public object OrderBy { get; set; }

        public int TotalUserCount { get; set; }

        public string CustomerId { get; set; }

        public int QuestionsCustom { get; set; }

        public int QuestionsSimulated { get; set; }

        public int QuestionsQuick { get; set; }

        public int QuestionsChallenged { get; set; }

        public int QuestionsSpacedRepetition { get; set; }

        //New Field added
        public int QuestionsSharedExam { get; set; }

    }
}
