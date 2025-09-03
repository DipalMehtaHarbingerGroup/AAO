namespace AAO.DTO.BCSCSelfAssessment
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExamDTO
    {
        public int ExamId { get; set; }

        [Display(Name = "Exam Title")]

        public string ExamName { get; set; }

        public string SearchTerm { get; set; }

        public System.DateTime ExamCreateDate { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public System.DateTime? EndDate { get; set; }

        [Display(Name = "Set Date Range")]
        public bool DateRangeEnabled { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailId { get; set; }

        public bool IsDeleted { get; set; }

        public int ExamType { get; set; }

        [Display(Name = "Challenge Mode")]
        public bool ExamMode { get; set; }

        public string AdditionalSettingExam { get; set; }

        public int UserId { get; set; }

        [Display(Name = "How Many Questions?")]
        [Required]
        public int NoofQuestions { get; set; }

        [Display(Name = "Timed Exam")]
        public bool ExamTimeType { get; set; }

        [Display(Name = "Show Correct Answers")]
        public bool ExamAnswerToShow { get; set; }

        [Display(Name = "Show Detailed Answers")]
        public bool ShowDetailedAnswers { get; set; }

        [Display(Name = "Show Answers After Exam End Date")]
        public bool ShowAnsAfterExamCompletion { get; set; }

        [Required]
        public List<string> TypeofCategoryList { get; set; }

        [Required]
        public IEnumerable<string> TypeofQuestionList { get; set; }

        [Required]
        public string TypeofCategory { get; set; }

        [Required]
        public string TypeofQuestion { get; set; }

        public int ExamStatus { get; set; }

        public int ExamAttemptId { get; set; }

        public int RoundNumber { get; set; }

        public int SessionId { get; set; }

        public string TypeofCategoryListstring { get; set; }

        public object SharedDate { get; set; }

        public int NumberOfCustomisedQuestions { get; set; }

        public string SelectedQuestions { get; set; }

        public List<int> SelectedQuestionIds { get; set; }

        public string QuestionListData { get; set; }

        public bool IsDateRangeValid()
        {
            if (!DateRangeEnabled)
                return true;

            if (!StartDate.HasValue || !EndDate.HasValue)
                return false;

            return EndDate.Value > StartDate.Value && StartDate.Value >= System.DateTime.Today;
        }

        public class QuestionItem
        {
            public int ExamId { get; set; }

            public int QuestionId { get; set; }

            public int ExamAttemptId { get; set; }

            public string AddedAt { get; set; }

            public string QuestionText { get; set; }

            public string QuestionDescription { get; set; }
        }
    }
}