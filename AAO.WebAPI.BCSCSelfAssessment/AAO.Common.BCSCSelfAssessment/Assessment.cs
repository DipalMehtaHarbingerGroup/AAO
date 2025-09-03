namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using AAO.DTO.BCSCSelfAssessment;

    public class Choice
    {
        public List<ChoiceImage> ChoiceImage { get; set; }

        public string choiceText { get; set; }

        public int choiceId { get; set; }

        public char choiceSequence { get; set; }
    }

    // public class QuestionChoice
    // {
    //    public int questionId { get; set; }

    // public List<Choice> choiceList { get; set; }
    // }

    // public class Question
    // {
    //  public int questionId { get; set; }
    // }
    public class QuestionImage
    {
        public int QuestionImageId { get; set; }

        public string Caption { get; set; }

        public string Comments { get; set; }

        public string Credit { get; set; }

        public string Figure { get; set; }

        public string ImagePath { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public int QuestionId { get; set; }
    }

    public class ChoiceImage
    {
        public int AnswerChoiceImageId { get; set; }

        public string Caption { get; set; }

        public string Comments { get; set; }

        public string Credit { get; set; }

        public string Figure { get; set; }

        public string ImagePath { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public int ChoiceId { get; set; }
    }

    public class QuestionDetails
    {
        public List<string> image_urls_fromexcerpt { get; set; }

        public List<QuestionImage> questionImage { get; set; }

        public int examId { get; set; }

        public int userId { get; set; }

        public int examStatus { get; set; }

        public int? questionId { get; set; }

        public int? previousId { get; set; }

        public int? nextId { get; set; }

        public int? serialNumber { get; set; }

        public string questionText { get; set; }

        public int rightChoiceId { get; set; }

        public List<Choice> choiceList { get; set; }

        public int examAttemptId { get; set; }

        public QuestionDiscussion questionDiscussion { get; set; }

        public List<string> references { get; set; }

        public List<PeerDetailVM> PeerDetails { get; set; }

        public List<string> notes { get; set; }

        public ExamAttemptAnswerDTO examAttemptAnswerDTO { get; set; }

        public bool examTimeType { get; set; }

        public bool examMode { get; set; }

        public bool examAnswerToShow { get; set; }

        public string examAttemptText { get; set; }

        public bool markCheck { get; set; }

        public int noOfQuestion { get; set; }

        public DateTime LastExamAttemptTime { get; set; }

        private DateTime CreatedOn = DateTime.Now;

        public DateTime CurrentExamAttemptTime
        {
            get { return (CreatedOn == DateTime.Now) ? DateTime.Now : CreatedOn; }
            set { CreatedOn = value; }
        }

        public string ExamName { get; set; }

        public int QuestionCount { get; set; }

        public int FriendlyId { get; set; }

        public List<string> BCSCExcerpt { get; set; }

        public string SubSpeciality { get; set; }

        public int Section { get; set; }

        public string TopicName { get; set; }

        public int ExamType { get; set; }

        public List<ExcerptHeading> ExcerptHeadings { get; set; }

        public int LastButtonClicked { get; set; }

        public bool NotRandomizeAnswer { get; set; }

        public List<string> RelatedContents { get; set; }
    }

    public class UpdateSkipAnswered
    {
        public int examId { get; set; }

        public int questionId { get; set; }

        public int selectedChoiceId { get; set; }

        public int? examAttemptId { get; set; }

        public bool? isRight { get; set; }

        public int userId { get; set; }

        public int currentQuestionId { get; set; }

        public string examAttemptText { get; set; }

        public char textType { get; set; }

        public bool markedStatus { get; set; }

        public int isYourAnswer { get; set; }

        public double consumedTimeDifference { get; set; }

        public string UserExamState { get; set; }

        public int ButtonType { get; set; }
    }

    public class QuestionDiscussion
    {
        public int discussionId { get; set; }

        public string discussionText { get; set; }

        public int questionId { get; set; }

        public bool discussionType { get; set; }

        public List<DiscussionImage> discussionImage { get; set; }
    }

    public class DiscussionImage
    {
        public int discussionImageId { get; set; }

        public string Caption { get; set; }

        public string Comments { get; set; }

        public string Credit { get; set; }

        public string Figure { get; set; }

        public string ImagePath { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public int discussionid { get; set; }
    }

    public class PeerDetailVM
    {
        public int ChoiceSequence { get; set; }

        public char ChoiceOption { get; set; }

        public int ChoiceId { get; set; }

        public bool IsRightChoice { get; set; }

        public double ChoicePercentage { get; set; }

        public int UserType { get; set; }
    }

    public class ViewProgressVM
    {
        public int QuestionId { get; set; }

        public int SelectedChoice { get; set; }

        public int CorrectChoice { get; set; }

        public bool IsRight { get; set; }

        public bool IsMarked { get; set; }

        public bool HasNotes { get; set; }

        public int QuestionSequence { get; set; }

        public char SelectedOption { get; set; }

        public char CorrectOption { get; set; }
    }

    public class AssesmentDetailVM
    {
        public int ExamId { get; set; }

        public string Filter { get; set; }

        public string SearchTerm { get; set; }

        public int QuestionId { get; set; }

        public int UserId { get; set; }

        public int NoOfRecords { get; set; }

        public int PageNo { get; set; }
    }

    public class QuestionIdWithCountVM
    {
        public int QuestionId { get; set; }

        public int QuestionIdCount { get; set; }

        public int QuestionNo { get; set; }
    }

    public class RightQuestionIdVM
    {
        public int RightQuestionId { get; set; }
    }

    public class QuestionData
    {
        public int QuestionId { get; set; }

        public int SubspecialtyId { get; set; }

        public List<Choice> ChoiceList { get; set; }

        public string Discussion { get; set; }

        public string References { get; set; }

        public int RightChoiceId { get; set; }

        public string RelatedContents { get; set; }
    }

    public class ExcerptHeading
    {
        public int HeadingId { get; set; }

        public string HeadingText { get; set; }

        public string Excerpts { get; set; }
    }

    public class QuestionIdWithExamId
    {
        public int QuestionId { get; set; }

        public int QuestionIdCount { get; set; }
        public int ExamId { get; set; }
        public int QuestionNo { get; set; }

    }
}