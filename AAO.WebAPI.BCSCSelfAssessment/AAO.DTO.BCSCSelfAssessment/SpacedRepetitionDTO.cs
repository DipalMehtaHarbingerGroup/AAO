namespace AAO.DTO.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SRPerformanceOverviewVM
    {
        public int SubspecialtyId { get; set; }

        public string SectionTitle { get; set; }

        public int SectionNumber { get; set; }

        public int CorrectConfident { get; set; }

        public int CorrectNotconfident { get; set; }

        public int IncorrectConfident { get; set; }

        public int IncorrectNotconfident { get; set; }

        public int QuestionCount { get; set; }

        public double CorrectConfidentPercent { get; set; }

        public double CorrectNotconfidentPercent { get; set; }

        public double IncorrectConfidentPercent { get; set; }

        public double IncorrectNotconfidentPercent { get; set; }

        public int UnAnswered { get; set; }

        public double UnAnsweredPercent { get; set; }
    }

    public class SRPerformanceOverviewChapterVM
    {
        public int SubspecialtyId { get; set; }

        public string ChapterNumber { get; set; }

        public string ChapterName { get; set; }

        public int CorrectConfident { get; set; }

        public int CorrectNotconfident { get; set; }

        public int IncorrectConfident { get; set; }

        public int IncorrectNotconfident { get; set; }

        public int QuestionCount { get; set; }

        public double CorrectConfidentPercent { get; set; }

        public double CorrectNotconfidentPercent { get; set; }

        public double IncorrectConfidentPercent { get; set; }

        public double IncorrectNotconfidentPercent { get; set; }

        public int UnAnswered { get; set; }

        public double UnAnsweredPercent { get; set; }
    }

    public class SRPerformanceOverviewTopicsVM
    {
        public int SubspecialtyId { get; set; }

        public string TopicName { get; set; }

        public string ChapterNumber { get; set; }

        public string ChapterName { get; set; }

        public int TopicId { get; set; }

        public int CorrectConfident { get; set; }

        public int CorrectNotconfident { get; set; }

        public int IncorrectConfident { get; set; }

        public int IncorrectNotconfident { get; set; }

        public int QuestionCount { get; set; }

        public double CorrectConfidentPercent { get; set; }

        public double CorrectNotconfidentPercent { get; set; }

        public double IncorrectConfidentPercent { get; set; }

        public double IncorrectNotconfidentPercent { get; set; }

        public int UnAnswered { get; set; }

        public double UnAnsweredPercent { get; set; }
    }

    public class 
        
        SRRecommendedTopicsVM
    {
        public string SectionName { get; set; }

        public string ChapterName { get; set; }

        public string TopicName { get; set; }

        public string Excerpt { get; set; }

        public int SectionNumber { get; set; }
        public int ExamId { get; set; }

    }

    public class SRRecommendedTopicsExportVM
    {
        public string SectionName { get; set; }

        public string ChapterName { get; set; }

        public string TopicName { get; set; }
    }
    public class SRRecommendedTopicsDeletedVM
    {
        public string SectionName { get; set; }

        public string ChapterName { get; set; }

        public string TopicName { get; set; }
        public int SectionNumber { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
    }

    public class SRRecommendedDeleteVM
    {
        public int ExamId { get; set; }
        public int ChoiceId { get; set; }

        public bool IsRight { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public int ExamAttemptId { get; set; }

        public int IsYourAnswer { get; set; }
    }
}