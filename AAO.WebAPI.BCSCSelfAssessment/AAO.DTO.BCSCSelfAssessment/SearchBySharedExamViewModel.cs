using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAO.DTO.BCSCSelfAssessment
{
    // Search parameters
    public class SearchBySharedExamViewModel
    {
        // Search fields
        public string SearchTerm { get; set; }
        public string Filter { get; set; }

        // Basic exam details
        public int selectedExamType { get; set; }
        public string ExamName { get; set; }
        public int? NoofQuestions { get; set; }

        // Exam settings (boolean fields)
        public bool ExamMode { get; set; }
        public bool ExamAnswerToShow { get; set; }
        public bool ExamTimeType { get; set; }

        // Shared exam specific fields
        public bool ShowDetailedAnswers { get; set; }
        public bool ShowAnsAfterExamCompletion { get; set; }
        public bool DateRangeEnabled { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EmailId { get; set; }
        public int? NumberOfCustomisedQuestions { get; set; }

        // Selected categories and question types (arrays)
        public string[] TypeofCategoryList { get; set; }
        public string[] TypeofQuestionList { get; set; }

        // Constructor to initialize arrays
        public SearchBySharedExamViewModel()
        {
            TypeofCategoryList = new string[0];
            TypeofQuestionList = new string[0];
        }
    }

    public class SearchResultsViewModel
    {
        public string SearchTerm { get; set; }
        public string Filter { get; set; }
        public IEnumerable<QuestionSearchResult> Questions { get; set; }
        public int TotalResults { get; set; }
        public QuestionSearchCriteria SearchCriteria { get; set; }
        public SearchBySharedExamViewModel OriginalExamData { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
    }

    public class QuestionSearchResult
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string SectionName { get; set; }
        public string QuestionType { get; set; }
        public string DifficultyLevel { get; set; }
        public string[] AnswerChoices { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
        public string Reference { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class QuestionSearchCriteria
    {
        public string SearchTerm { get; set; }
        public string Filter { get; set; }
        public int ExamType { get; set; }
        public string[] SelectedSections { get; set; }
        public string[] SelectedQuestionTypes { get; set; }
        public bool ExamMode { get; set; }
        public int MaxResults { get; set; } = 100;

        // Additional search filters
        public string DifficultyLevel { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        // Basic exam details
        public int selectedExamType { get; set; }
        public string ExamName { get; set; }
        public int? NoofQuestions { get; set; }

        // Exam settings (boolean fields)
        public bool ExamAnswerToShow { get; set; }
        public bool ExamTimeType { get; set; }

        // Shared exam specific fields
        public bool ShowDetailedAnswers { get; set; }
        public bool ShowAnsAfterExamCompletion { get; set; }
        public bool DateRangeEnabled { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EmailId { get; set; }
        public int? NumberOfCustomisedQuestions { get; set; }
    }
}
