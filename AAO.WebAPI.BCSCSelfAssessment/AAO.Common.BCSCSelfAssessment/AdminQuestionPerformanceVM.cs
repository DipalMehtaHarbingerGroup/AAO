namespace AAO.Common.BCSCSelfAssessment
{
    using System;

    public class AdminQuestionPerformanceVM
    {
        public string Stem { get; set; }

        public string Chapter { get; set; }

        public DateTime? ExamStartDate { get; set; }

        public DateTime? ExamCompletedDate { get; set; }

        public string Subspecialty { get; set; }

        public int QuestionId { get; set; }

        public int SubspecialtyId { get; set; }

        public string Topic { get; set; }

        public int UserAnsweredcorrectly { get; set; }

        public int UserAnsweredincorrectly { get; set; }

        public int UsersRepeatedSRMode { get; set; }

        public string Percentagecorrectly { get; set; }

        public string PercentageIncorrectly { get; set; }

        public string CorrectAnswer { get; set; }

        public string Distractor_1 { get; set; }

        public int User_chose_distractor_1 { get; set; }

        public string Distractor_2 { get; set; }

        public int User_chose_distractor_2 { get; set; }

        public string Distractor_3 { get; set; }

        public int User_chose_distractor_3 { get; set; }

        public int Year { get; set; }

        public int NoOfRecords { get; set; }

        public int PageNo { get; set; }

        public string OrderBy { get; set; }

        public string Seq { get; set; }

        public bool OptOutValue { get; set; }

        public int StartCount { get; set; }

        public int EndCount { get; set; }

        public string Avg_times_incorrect_before_correct { get; set; }

        public int Users_correct_first_try { get; set; }
    }
}
