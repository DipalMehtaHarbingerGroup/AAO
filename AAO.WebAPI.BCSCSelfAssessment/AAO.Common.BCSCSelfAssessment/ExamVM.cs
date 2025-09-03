namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;

    public class ExamStatusCountVM
    {
        public int allExam { get; set; }

        public int completed { get; set; }

        public int inProgress { get; set; }

        public int notStarted { get; set; }
    }

    public class ExamStatusCountSRVM
    {
        public int TopicCount { get; set; }

        public int TotalAnsweredQuestions { get; set; }

        public int AnsweredCorrectly { get; set; }

        public int AnsweredInCorrectly { get; set; }
    }

    public class ExamScoreDetailVM
    {
        public int examType { get; set; }

        public int averageScore { get; set; }

        public int totalAttempedQuestion { get; set; }

        public int correctAnswer { get; set; }

        public int inCorrectAnswer { get; set; }
    }

    public class ServiceCallVM
    {
        public int SubpecialityId { get; set; }

        public string ChapterNumber { get; set; }

        public string ChapterName { get; set; }

        public int examId { get; set; }

        public int questionId { get; set; }

        public int examAttemptId { get; set; }

        public int userId { get; set; }

        public int examType { get; set; }

        public int Status { get; set; }

        public DateTime Modifiedafter { get; set; }

        public string OptIn { get; set; }
    }

    public class ExamHistoryVM
    {
        public int ExamId { get; set; }

        public string ExamName { get; set; }

        public int AttempedQuestionCount { get; set; }

        public int TotalQuestionCount { get; set; }

        public string ExamType { get; set; }

        public string LastAttempedDate { get; set; }

        public int LastAttempedQuestionId { get; set; }

        public int NextAttemptQuestionId { get; set; }

        public int ExamCount { get; set; }
    }

    public class ExamNameVM
    {
        public int UserId { get; set; }

        public string ExamName { get; set; }
    }

    public class ExamCountOnExamTypeVM
    {
        public int UserId { get; set; }

        public int ExamType { get; set; }

        public int CustomModeCount { get; set; }

        public int SimulatedModeCount { get; set; }

        public int QuickModeCount { get; set; }

        //NEw field added
        public int SharedModeCount { get; set; }

    }

    public class ExamQuestionTypeCountOnCategoryVM
    {
        public int UserId { get; set; }

        public int CategoryId { get; set; }
    }

    public class QuestionCountOnSection
    {
        public int UserId { get; set; }

        public string SectionId { get; set; }

        public int TotalCount { get; set; }

        public int InCorrectCount { get; set; }

        public int SkipCount { get; set; }

        public int MarkCount { get; set; }
    }

    public class PdfDetailsDataVM
    {
        public string CompletionDate { get; set; }

        public string UserName { get; set; }

        public string ResidencyEndDate { get; set; }

        public int TotalQuestion { get; set; }

        public List<SectionVM> SectionLIst { get; set; }

        public string TimedExam { get; set; }

        public string ExamMode { get; set; }

        public int OverallScore { get; set; }

        public int CorrectAnswers { get; set; }

        public int IncorrectAnswers { get; set; }

        public int Unanswered { get; set; }
    }

    public class SectionVM
    {
        public int SectionId { get; set; }

        public string SectionName { get; set; }
    }

    public class LoginVM
    {
        public int userId { get; set; }

    }
}
